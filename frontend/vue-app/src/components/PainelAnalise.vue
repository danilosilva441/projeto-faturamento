<script setup lang="ts">
import { ref } from 'vue';
// Importa as nossas ferramentas: a "forma" do objeto Operacao e a função para pedir a análise
import type { Operacao, ResultadoAnalise } from '../types';
import { getMediaDiaria } from '../services/apiService';

// Define que este componente espera receber a lista de operações do seu "chefe" (App.vue)
defineProps<{
  operacoes: Operacao[]
}>();

// --- Variáveis de Estado para controlar o componente ---
const operacaoSelecionadaId = ref<number | null>(null);
const resultado = ref<ResultadoAnalise | null>(null);
const carregando = ref(false);
const erro = ref<string | null>(null);

// Função chamada quando o botão "Calcular Média" é clicado
async function calcularMedia() {
  // Validação: Garante que o utilizador escolheu uma operação
  if (!operacaoSelecionadaId.value) {
    erro.value = 'Por favor, selecione uma operação para calcular.';
    return;
  }

  // Prepara o componente para a chamada de API
  carregando.value = true;
  erro.value = null;
  resultado.value = null;

  try {
    // Chama a nossa nova função do apiService
    const data = await getMediaDiaria(operacaoSelecionadaId.value);
    resultado.value = data; // Guarda o resultado para ser exibido na tela
  } catch (error: any) {
    console.error('Erro ao calcular média:', error);
    erro.value = error.response?.data || 'Não foi possível calcular a média.';
  } finally {
    carregando.value = false; // Termina o estado de "carregando"
  }
}

// Função auxiliar para formatar o resultado como moeda
function formatarMoeda(valor: number) {
  return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}
</script>

<template>
  <div class="bg-white p-6 rounded-xl shadow-lg transition-shadow hover:shadow-2xl">
    <h2 class="text-2xl font-bold text-gray-800 mb-4 border-b pb-2">Análise de Faturamento</h2>
    
    <div class="space-y-4">
      <p class="text-sm text-gray-600">Selecione uma operação para calcular a média de faturamento diário com base nos dados históricos.</p>
      
      <!-- Dropdown e Botão -->
      <div class="flex flex-col sm:flex-row items-center gap-4">
        <select 
          v-model="operacaoSelecionadaId"
          class="block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500"
        >
          <option :value="null" disabled>Selecione uma operação</option>
          <option v-for="op in operacoes" :key="op.id" :value="op.id">
            {{ op.nome }}
          </option>
        </select>

        <button 
          @click="calcularMedia" 
          :disabled="carregando"
          class="w-full sm:w-auto flex justify-center items-center py-2 px-6 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:bg-gray-400"
        >
          <span v-if="!carregando">Calcular Média</span>
          <span v-else>Calculando...</span>
        </button>
      </div>

      <!-- Área de Resultados -->
      <div v-if="resultado" class="mt-4 p-4 bg-indigo-50 border-l-4 border-indigo-500 text-indigo-800 rounded-r-lg">
        <p class="font-bold">Média de Faturamento Diário:</p>
        <p class="text-2xl font-mono">{{ formatarMoeda(resultado.mediaCalculada) }}</p>
      </div>

      <!-- Mensagem de Erro -->
      <div v-if="erro" class="mt-4 p-2 text-center bg-red-100 text-red-700 rounded-md">
        {{ erro }}
      </div>
    </div>
  </div>
</template>
