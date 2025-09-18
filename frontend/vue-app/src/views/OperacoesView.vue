<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import type { Operacao } from '../types';
import { getOperacoes, createOperacao, updateOperacao, deleteOperacao, type SalvarOperacaoPayload } from '../services/apiService';

const operacoes = ref<Operacao[]>([]);
const carregando = ref(true);
const erro = ref<string | null>(null);

const showModal = ref(false);
const operacaoEmEdicao = ref<Partial<Operacao>>({});
const isEditing = ref(false);

async function buscarOperacoes() {
  carregando.value = true;
  try {
    operacoes.value = await getOperacoes();
  } catch (e) {
    erro.value = "Não foi possível carregar as operações.";
  } finally {
    carregando.value = false;
  }
}

onMounted(buscarOperacoes);

function abrirModalParaCriar() {
  isEditing.value = false;
  operacaoEmEdicao.value = { nome: '', descricao: '', metaMensal: 0 };
  showModal.value = true;
}

function abrirModalParaEditar(op: Operacao) {
  isEditing.value = true;
  operacaoEmEdicao.value = { ...op };
  showModal.value = true;
}

function fecharModal() {
  showModal.value = false;
}

async function handleSalvarOperacao() {
  const payload: SalvarOperacaoPayload = {
    nome: operacaoEmEdicao.value.nome || '',
    descricao: operacaoEmEdicao.value.descricao || null,
    metaMensal: operacaoEmEdicao.value.metaMensal || 0
  };

  if (isEditing.value) {
    if (!operacaoEmEdicao.value.id) return;
    await updateOperacao(operacaoEmEdicao.value.id, payload);
  } else {
    await createOperacao(payload);
  }
  
  fecharModal();
  buscarOperacoes();
}

async function handleEncerrarOperacao(id: number) {
  if (confirm("Tem a certeza de que deseja encerrar esta operação? Ela não poderá mais receber lançamentos.")) {
    await deleteOperacao(id);
    buscarOperacoes();
  }
}

const tituloModal = computed(() => isEditing.value ? 'Editar Operação' : 'Adicionar Nova Operação');

function formatarMoeda(valor: number) {
  return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex justify-between items-center">
      <h1 class="text-3xl font-bold text-gray-800">Gestão de Operações</h1>
      <button @click="abrirModalParaCriar" class="py-2 px-4 bg-indigo-600 text-white font-semibold rounded-lg shadow-md hover:bg-indigo-700">
        Adicionar Operação
      </button>
    </div>

    <div class="bg-white p-6 rounded-lg shadow-md">
      <div v-if="carregando" class="text-center py-8">A carregar operações...</div>
      <div v-else class="overflow-x-auto">
        <table class="min-w-full bg-white">
          <thead class="bg-gray-800 text-white">
            <tr>
              <th class="py-3 px-4 uppercase font-semibold text-sm text-left">Nome</th>
              <th class="py-3 px-4 uppercase font-semibold text-sm text-right">Meta Mensal</th>
              <th class="py-3 px-4 uppercase font-semibold text-sm text-center">Ações</th>
            </tr>
          </thead>
          <tbody class="text-gray-700">
            <tr v-for="op in operacoes" :key="op.id" class="border-b hover:bg-gray-50">
              <td class="py-3 px-4">{{ op.nome }}</td>
              <td class="py-3 px-4 text-right font-mono">{{ formatarMoeda(op.metaMensal) }}</td>
              <td class="py-3 px-4 text-center">
                <button @click="abrirModalParaEditar(op)" class="text-indigo-600 hover:text-indigo-900 mr-4">Editar</button>
                <button @click="handleEncerrarOperacao(op.id)" class="text-red-600 hover:text-red-900">Encerrar</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Modal para Criar/Editar Operação -->
    <div v-if="showModal" class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
      <div class="bg-white p-8 rounded-lg shadow-xl w-full max-w-md">
        <h2 class="text-2xl font-bold mb-4">{{ tituloModal }}</h2>
        <form @submit.prevent="handleSalvarOperacao" class="space-y-4">
          <div>
            <label for="op-nome" class="block text-sm font-medium">Nome da Operação</label>
            <input type="text" id="op-nome" v-model="operacaoEmEdicao.nome" class="mt-1 block w-full p-2 border rounded-md" />
          </div>
          <div>
            <label for="op-desc" class="block text-sm font-medium">Descrição</label>
            <textarea id="op-desc" v-model="operacaoEmEdicao.descricao" rows="2" class="mt-1 block w-full p-2 border rounded-md"></textarea>
          </div>
          <div>
            <label for="op-meta" class="block text-sm font-medium">Meta Mensal (R$)</label>
            <input type="number" step="0.01" id="op-meta" v-model.number="operacaoEmEdicao.metaMensal" class="mt-1 block w-full p-2 border rounded-md" />
          </div>
          <div class="flex justify-end gap-4 mt-6">
            <button type="button" @click="fecharModal" class="py-2 px-4 bg-gray-200 text-gray-800 rounded-lg">Cancelar</button>
            <button type="submit" class="py-2 px-4 bg-indigo-600 text-white rounded-lg">Salvar</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

