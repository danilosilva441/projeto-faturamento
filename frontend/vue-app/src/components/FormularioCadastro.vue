<script setup lang="ts">
import { ref } from 'vue';
// Importa a "forma" do objeto Operacao do nosso dicionário de tipos
import type { Operacao } from '../types';
// Importa a função específica para criar faturamentos do nosso serviço de API
import { createFaturamento } from '../services/apiService';

// Define as propriedades que este componente recebe do seu "chefe" (o App.vue)
defineProps<{
  operacoes: Operacao[]
}>();

// Define os "eventos" que este componente pode enviar de volta para o seu chefe
const emit = defineEmits(['faturamento-cadastrado']);

// Variáveis para controlar o formulário
const operacaoSelecionadaId = ref<number | null>(null);
const novaData = ref(new Date().toISOString().split('T')[0]); // Já vem com a data de hoje
const novoValor = ref<number | null>(null);
const enviando = ref(false);
const mensagemSucesso = ref('');
const mensagemErro = ref('');

// Função chamada quando o botão de submeter é clicado
async function handleSubmit() {
  // Validação básica
  if (!operacaoSelecionadaId.value || !novaData.value || !novoValor.value) {
    mensagemErro.value = 'Todos os campos são obrigatórios.';
    return;
  }

  enviando.value = true;
  mensagemErro.value = '';
  mensagemSucesso.value = '';

  try {
    // Usa a nossa nova função centralizada para chamar a API
    await createFaturamento({
      operacaoId: operacaoSelecionadaId.value,
      dataRef: new Date(novaData.value).toISOString(),
      valor: novoValor.value
    });

    mensagemSucesso.value = 'Faturamento cadastrado com sucesso!';
    // AVISA AO CHEFE (App.vue) que o cadastro deu certo!
    emit('faturamento-cadastrado');

    // Limpa o formulário para o próximo lançamento
    novoValor.value = null;
    operacaoSelecionadaId.value = null;

  } catch (error: any) {
    // Se a API retornar um erro, mostra a mensagem que o backend enviou
    mensagemErro.value = error.response?.data || 'Ocorreu um erro inesperado ao cadastrar.';
  } finally {
    enviando.value = false;
  }
}
</script>

<template>
  <div class="bg-white p-6 rounded-xl shadow-lg transition-shadow hover:shadow-2xl">
    <h2 class="text-2xl font-bold text-gray-800 mb-4 border-b pb-2">Cadastrar Lançamento</h2>
    <form @submit.prevent="handleSubmit" class="space-y-4">
      <div>
        <label for="operacao" class="block text-sm font-medium text-gray-700">Operação</label>
        <select id="operacao" v-model="operacaoSelecionadaId" class="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500">
          <option disabled :value="null">Selecione uma operação</option>
          <option v-for="op in operacoes" :key="op.id" :value="op.id">{{ op.nome }}</option>
        </select>
      </div>
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div>
          <label for="data" class="block text-sm font-medium text-gray-700">Data</label>
          <input type="date" id="data" v-model="novaData" class="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500">
        </div>
        <div>
          <label for="valor" class="block text-sm font-medium text-gray-700">Valor (R$)</label>
          <input type="number" step="0.01" id="valor" v-model="novoValor" placeholder="1500.50" class="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500">
        </div>
      </div>
      <button type="submit" :disabled="enviando" class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:bg-gray-400">
        <span v-if="!enviando">Cadastrar</span>
        <span v-else>Enviando...</span>
      </button>
      <div v-if="mensagemSucesso" class="mt-4 text-center p-2 bg-green-100 text-green-700 rounded-md">{{ mensagemSucesso }}</div>
      <div v-if="mensagemErro" class="mt-4 text-center p-2 bg-red-100 text-red-700 rounded-md">{{ mensagemErro }}</div>
    </form>
  </div>
</template>

