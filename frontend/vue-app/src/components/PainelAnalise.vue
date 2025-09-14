<script setup lang="ts">
import { ref } from 'vue';
// Importa todas as nossas ferramentas de tipo e de API
import type { Operacao, ResultadoAnalise, ResultadoPrevisao } from '../types';
import { getMediaDiaria, getPrevisaoFaturamento } from '../services/apiService';

// Define que este componente espera receber a lista de operações do seu "chefe" (App.vue)
defineProps<{
  operacoes: Operacao[]
}>();

// --- Variáveis de Estado para controlar o componente ---
const operacaoSelecionadaId = ref<number | null>(null);
const resultadoMedia = ref<ResultadoAnalise | null>(null);
const resultadoPrevisao = ref<ResultadoPrevisao | null>(null); // Nova variável para a previsão
const carregandoMedia = ref(false);
const carregandoPrevisao = ref(false); // Novo estado de carregamento para a previsão
const erro = ref<string | null>(null);

// --- Funções de Lógica ---
async function calcularMedia() {
  if (!operacaoSelecionadaId.value) {
    erro.value = 'Selecione uma operação.'; return;
  }
  carregandoMedia.value = true; erro.value = null; resultadoMedia.value = null;
  try {
    resultadoMedia.value = await getMediaDiaria(operacaoSelecionadaId.value);
  } catch (e: any) {
    erro.value = e.response?.data || 'Erro ao calcular média.';
  } finally {
    carregandoMedia.value = false;
  }
}

// Nova função para gerar a previsão
async function gerarPrevisao() {
  if (!operacaoSelecionadaId.value) {
    erro.value = 'Selecione uma operação.'; return;
  }
  carregandoPrevisao.value = true; erro.value = null; resultadoPrevisao.value = null;
  try {
    resultadoPrevisao.value = await getPrevisaoFaturamento(operacaoSelecionadaId.value);
  } catch (e: any) {
    erro.value = e.response?.data || 'Erro ao gerar previsão.';
  } finally {
    carregandoPrevisao.value = false;
  }
}

function formatarMoeda(valor: number) {
  return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}
</script>

<template>
  <div class="bg-white p-6 rounded-xl shadow-lg transition-shadow hover:shadow-2xl">
    <h2 class="text-2xl font-bold text-gray-800 mb-4 border-b pb-2">Análise e Previsões</h2>
    
    <div class="space-y-4">
      <p class="text-sm text-gray-600">Selecione uma operação para executar cálculos com base nos dados históricos.</p>
      
      <!-- Seletor de Operação -->
      <div>
        <label for="analise-operacao" class="block text-sm font-medium text-gray-700">Operação</label>
        <select 
          id="analise-operacao"
          v-model="operacaoSelecionadaId"
          class="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500"
        >
          <option :value="null" disabled>Selecione uma operação</option>
          <option v-for="op in operacoes" :key="op.id" :value="op.id">
            {{ op.nome }}
          </option>
        </select>
      </div>

      <!-- Botões de Ação -->
      <div class="flex flex-wrap gap-4">
        <button 
          @click="calcularMedia" 
          :disabled="carregandoMedia"
          class="flex-1 min-w-[150px] justify-center py-2 px-4 border rounded-md shadow-sm text-sm font-medium text-indigo-700 bg-indigo-100 hover:bg-indigo-200 disabled:bg-gray-100"
        >
          <span v-if="!carregandoMedia">Calcular Média</span>
          <span v-else>Calculando...</span>
        </button>
        <button 
          @click="gerarPrevisao" 
          :disabled="carregandoPrevisao"
          class="flex-1 min-w-[150px] justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 disabled:bg-gray-400"
        >
          <span v-if="!carregandoPrevisao">Gerar Previsão (7 dias)</span>
          <span v-else>Gerando...</span>
        </button>
      </div>

      <!-- Área de Resultados -->
      <div v-if="resultadoMedia" class="mt-4 p-4 bg-indigo-50 border-l-4 border-indigo-500 text-indigo-800 rounded-r-lg">
        <p class="font-bold">Média de Faturamento Diário:</p>
        <p class="text-2xl font-mono">{{ formatarMoeda(resultadoMedia.mediaCalculada) }}</p>
      </div>
      
      <!-- NOVA SECÇÃO DE PREVISÃO -->
      <div v-if="resultadoPrevisao" class="mt-4 p-4 bg-green-50 border-l-4 border-green-500 rounded-r-lg">
        <p class="font-bold text-green-800">Previsão de Faturamento (Próximos 7 dias):</p>
        <ul class="mt-2 space-y-1 text-sm text-green-700">
          <li v-for="item in resultadoPrevisao.previsao" :key="item.data" class="flex justify-between">
            <span>{{ new Date(item.data).toLocaleDateString('pt-BR', { weekday: 'long', day: '2-digit', month: '2-digit' }) }}:</span>
            <span class="font-mono font-semibold">{{ formatarMoeda(item.valorPrevisto) }}</span>
          </li>
        </ul>
      </div>

      <div v-if="erro" class="mt-4 p-2 text-center bg-red-100 text-red-700 rounded-md">{{ erro }}</div>
    </div>
  </div>
</template>

