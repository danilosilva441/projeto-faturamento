<script setup lang="ts">
import { ref } from 'vue';
import axios from 'axios';

// --- DEFINIÇÃO DAS PROPRIEDADES E EVENTOS ---
// defineProps: Define os "dados" que este componente RECEBE do componente pai.
// Neste caso, ele precisa da lista de operações para popular o dropdown.
defineProps<{
  operacoes: { id: number, nome: string }[]
}>();

// defineEmits: Define os "eventos" que este componente ENVIA para o componente pai.
// Quando um faturamento for cadastrado, ele vai "gritar" o evento 'faturamentoCadastrado'.
const emit = defineEmits(['faturamentoCadastrado']);

// --- ESTADO INTERNO DO FORMULÁRIO ---
const API_URL = 'http://localhost:5013/api';
const operacaoSelecionadaId = ref<number | null>(null);
const novaData = ref<string>('');
const novoValor = ref<number | null>(null);
const enviandoFormulario = ref<boolean>(false);
const mensagemSucesso = ref<string | null>(null);
const mensagemErroFormulario = ref<string | null>(null);

// --- LÓGICA DE CADASTRO ---
async function cadastrarFaturamento() {
  mensagemSucesso.value = null;
  mensagemErroFormulario.value = null;

  if (!operacaoSelecionadaId.value || !novaData.value || !novoValor.value) {
    mensagemErroFormulario.value = "Por favor, preencha todos os campos.";
    return;
  }
  
  enviandoFormulario.value = true;

  const faturamentoDto = {
    operacaoId: operacaoSelecionadaId.value,
    dataRef: novaData.value,
    valor: novoValor.value
  };

  try {
    const response = await axios.post(`${API_URL}/faturamentos`, faturamentoDto);
    mensagemSucesso.value = `Faturamento de R$ ${response.data.valor} cadastrado com sucesso!`;
    
    // AVISA O PAI QUE UM NOVO FATURAMENTO FOI CRIADO!
    emit('faturamentoCadastrado', response.data);

    operacaoSelecionadaId.value = null;
    novaData.value = '';
    novoValor.value = null;

  } catch (error: any) {
    mensagemErroFormulario.value = error.response ? error.response.data : "Erro de conexão.";
  } finally {
    enviandoFormulario.value = false;
  }
}
</script>

<template>
  <div class="bg-white p-6 rounded-xl shadow-lg transition-shadow hover:shadow-2xl">
    <h2 class="text-2xl font-bold text-gray-800 mb-4">Cadastrar Novo Lançamento</h2>
    
    <form @submit.prevent="cadastrarFaturamento">
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
        
        <div>
          <label for="operacao" class="block text-sm font-medium text-gray-700">Operação</label>
          <select id="operacao" v-model="operacaoSelecionadaId" class="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500">
            <option :value="null" disabled>Selecione uma operação</option>
            <option v-for="op in operacoes" :key="op.id" :value="op.id">
              {{ op.nome }}
            </option>
          </select>
        </div>

        <div>
          <label for="data" class="block text-sm font-medium text-gray-700">Data</label>
          <input type="date" id="data" v-model="novaData" class="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500">
        </div>

        <div>
          <label for="valor" class="block text-sm font-medium text-gray-700">Valor (R$)</label>
          <input type="number" id="valor" v-model="novoValor" step="0.01" placeholder="1500.50" class="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500">
        </div>

      </div>

      <div class="mt-6 text-right">
        <button type="submit" :disabled="enviandoFormulario" class="inline-flex items-center justify-center py-2 px-4 border border-transparent shadow-sm text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:bg-gray-400">
          <svg v-if="enviandoFormulario" class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
            <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
          </svg>
          {{ enviandoFormulario ? 'Enviando...' : 'Cadastrar' }}
        </button>
      </div>
    </form>

    <div v-if="mensagemSucesso" class="mt-4 p-3 bg-green-100 text-green-800 border-l-4 border-green-500 rounded-md transition-opacity duration-300">
      {{ mensagemSucesso }}
    </div>
    <div v-if="mensagemErroFormulario" class="mt-4 p-3 bg-red-100 text-red-800 border-l-4 border-red-500 rounded-md transition-opacity duration-300">
      {{ mensagemErroFormulario }}
    </div>
  </div>
</template>
