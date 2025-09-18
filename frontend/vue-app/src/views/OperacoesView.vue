<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
// Importa as nossas ferramentas: o "dicionário" de tipos e as funções do serviço de API
import type { Operacao } from '../types';
import { getOperacoes, createOperacao, updateOperacao, deleteOperacao, type SalvarOperacaoPayload } from '../services/apiService';

// --- ESTADO DA PÁGINA ---
const operacoes = ref<Operacao[]>([]);
const carregando = ref(true);
const erro = ref<string | null>(null);

// --- ESTADO DO MODAL (FORMULÁRIO) ---
const showModal = ref(false); // Controla se a janela do formulário está visível
const operacaoEmEdicao = ref<Partial<Operacao>>({}); // Guarda os dados do formulário
const isEditing = ref(false); // Controla se estamos a "Criar" ou a "Editar"

// --- LÓGICA DE DADOS ---
// Função para buscar as operações da API e atualizar a nossa lista
async function buscarOperacoes() {
  carregando.value = true;
  erro.value = null;
  try {
    operacoes.value = await getOperacoes();
  } catch (e) {
    erro.value = "Não foi possível carregar as operações.";
    console.error(e);
  } finally {
    carregando.value = false;
  }
}

// Busca os dados iniciais assim que a página é carregada
onMounted(buscarOperacoes);


// --- LÓGICA DO MODAL E DO FORMULÁRIO ---
function abrirModalParaCriar() {
  isEditing.value = false;
  // Prepara um objeto limpo para o formulário de criação
  operacaoEmEdicao.value = { nome: '', descricao: '', metaMensal: 0 };
  showModal.value = true;
}

function abrirModalParaEditar(op: Operacao) {
  isEditing.value = true;
  // Clona os dados da operação selecionada para o formulário de edição
  operacaoEmEdicao.value = { ...op };
  showModal.value = true;
}

function fecharModal() {
  showModal.value = false;
}

// Função chamada quando o botão "Salvar" do formulário é clicado
async function handleSalvarOperacao() {
  // Prepara o "payload" (os dados a serem enviados) com base no que foi preenchido
  const payload: SalvarOperacaoPayload = {
    nome: operacaoEmEdicao.value.nome || '',
    descricao: operacaoEmEdicao.value.descricao || null,
    metaMensal: operacaoEmEdicao.value.metaMensal || 0
  };

  try {
    if (isEditing.value) {
      // Se estiver a editar, chama a função de atualização
      if (!operacaoEmEdicao.value.id) return;
      await updateOperacao(operacaoEmEdicao.value.id, payload);
    } else {
      // Se estiver a criar, chama a função de criação
      await createOperacao(payload);
    }
    fecharModal();
    buscarOperacoes(); // Recarrega a lista para mostrar as alterações
  } catch (error) {
    console.error("Erro ao salvar operação:", error);
    alert("Ocorreu um erro ao salvar a operação. Verifique o console.");
  }
}

// Função chamada quando o botão "Encerrar" é clicado
async function handleEncerrarOperacao(id: number) {
  if (confirm("Tem a certeza de que deseja encerrar esta operação? Ela não poderá mais receber lançamentos.")) {
    try {
      await deleteOperacao(id); // Chama a função de "soft delete" da API
      buscarOperacoes(); // Recarrega a lista
    } catch (error) {
      console.error("Erro ao encerrar operação:", error);
      alert("Ocorreu um erro ao encerrar a operação.");
    }
  }
}

// Propriedade "computada" que muda o título do modal dinamicamente
const tituloModal = computed(() => isEditing.value ? 'Editar Operação' : 'Adicionar Nova Operação');

// Função de formatação de moeda
function formatarMoeda(valor: number) {
  return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}
</script>

<template>
  <div class="space-y-6">
    <!-- Cabeçalho da Página com o botão de Adicionar -->
    <div class="flex justify-between items-center">
      <h1 class="text-3xl font-bold text-gray-800">Gestão de Operações</h1>
      <button @click="abrirModalParaCriar" class="py-2 px-4 bg-indigo-600 text-white font-semibold rounded-lg shadow-md hover:bg-indigo-700">
        Adicionar Operação
      </button>
    </div>

    <!-- Tabela para listar as Operações -->
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

    <!-- Janela Modal para Criar/Editar Operação -->
    <div v-if="showModal" class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
      <div class="bg-white p-8 rounded-lg shadow-xl w-full max-w-md">
        <h2 class="text-2xl font-bold mb-4">{{ tituloModal }}</h2>
        <form @submit.prevent="handleSalvarOperacao" class="space-y-4">
          <div>
            <label for="op-nome" class="block text-sm font-medium">Nome da Operação</label>
            <input type="text" id="op-nome" v-model="operacaoEmEdicao.nome" class="mt-1 block w-full p-2 border rounded-md" required />
          </div>
          <div>
            <label for="op-desc" class="block text-sm font-medium">Descrição</label>
            <textarea id="op-desc" v-model="operacaoEmEdicao.descricao" rows="2" class="mt-1 block w-full p-2 border rounded-md"></textarea>
          </div>
          <div>
            <label for="op-meta" class="block text-sm font-medium">Meta Mensal (R$)</label>
            <input type="number" step="0.01" id="op-meta" v-model.number="operacaoEmEdicao.metaMensal" class="mt-1 block w-full p-2 border rounded-md" required />
          </div>
          <div class="flex justify-end gap-4 mt-6">
            <button type="button" @click="fecharModal" class="py-2 px-4 bg-gray-200 text-gray-800 rounded-lg hover:bg-gray-300">Cancelar</button>
            <button type="submit" class="py-2 px-4 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700">Salvar</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

