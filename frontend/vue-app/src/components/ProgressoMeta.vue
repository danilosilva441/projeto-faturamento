<script setup lang="ts">
import { ref } from 'vue';
// Importa as nossas ferramentas: a "forma" dos objetos e a função para pedir a análise
import type { Operacao, ProgressoMeta } from '../types';
import { getProgressoMeta } from '../services/apiService';

// Define que este componente espera receber a lista de operações do seu "chefe" (HomeView.vue)
defineProps<{
  operacoes: Operacao[]
}>();

// --- Variáveis de Estado para controlar o componente ---
const operacaoSelecionadaId = ref<number | null>(null);
const resultado = ref<ProgressoMeta | null>(null);
const carregando = ref(false);
const erro = ref<string | null>(null);

// Função chamada quando o utilizador seleciona uma operação no dropdown
async function buscarProgresso() {
  // Validação: Garante que o utilizador escolheu uma operação
  if (!operacaoSelecionadaId.value) {
    resultado.value = null; // Limpa o resultado se nenhuma operação for selecionada
    return;
  }

  // Prepara o componente para a chamada de API
  carregando.value = true;
  erro.value = null;
  resultado.value = null;

  try {
    // Chama a nossa nova função do apiService
    const data = await getProgressoMeta(operacaoSelecionadaId.value);
    resultado.value = data; // Guarda o resultado para ser exibido na tela
  } catch (error: any) {
    console.error('Erro ao buscar progresso da meta:', error);
    erro.value = error.response?.data || 'Não foi possível buscar o progresso da meta.';
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
    <h2 class="text-2xl font-bold text-gray-800 mb-4 border-b pb-2">Progresso da Meta Mensal</h2>
    
    <div class="space-y-4">
      <!-- Dropdown para selecionar a Operação -->
      <div>
        <label for="progresso-operacao" class="block text-sm font-medium text-gray-700">Selecione uma Operação</label>
        <select 
          id="progresso-operacao"
          v-model="operacaoSelecionadaId"
          @change="buscarProgresso"
          class="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500"
        >
          <option :value="null" disabled>-- Ver progresso para --</option>
          <option v-for="op in operacoes" :key="op.id" :value="op.id">
            {{ op.nome }}
          </option>
        </select>
      </div>

      <!-- Área de Resultados -->
      <div v-if="carregando" class="text-center py-4 text-gray-500">
        A calcular progresso...
      </div>
      
      <div v-else-if="resultado" class="mt-4 space-y-3">
        <!-- Barra de Progresso Visual -->
        <div>
          <div class="flex justify-between mb-1 text-sm font-medium text-gray-700">
            <span>{{ formatarMoeda(resultado.totalFaturado) }}</span>
            <span class="font-bold">{{ formatarMoeda(resultado.metaMensal) }}</span>
          </div>
          <div class="w-full bg-gray-200 rounded-full h-4">
            <div 
              class="bg-green-500 h-4 rounded-full" 
              :style="{ width: Math.min(resultado.progressoPercentual, 100) + '%' }"
            ></div>
          </div>
          <div class="text-right mt-1 text-sm font-semibold text-green-700">
            {{ resultado.progressoPercentual }}% Atingido
          </div>
        </div>
        
        <!-- Detalhes Adicionais -->
        <div class="text-center text-sm text-gray-600">
            <span v-if="resultado.valorRestante > 0">
                Faltam <strong>{{ formatarMoeda(resultado.valorRestante) }}</strong> para atingir a meta de {{ resultado.mesReferencia }}.
            </span>
            <span v-else class="text-green-600 font-bold">
                🎉 Meta atingida e superada!
            </span>
        </div>
      </div>

      <div v-if="erro" class="mt-4 p-2 text-center bg-red-100 text-red-700 rounded-md">
        {{ erro }}
      </div>
    </div>
  </div>
</template>
