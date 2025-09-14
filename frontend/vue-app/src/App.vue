<script setup lang="ts">
import { ref, onMounted } from 'vue';
import type { Operacao, Faturamento } from '../types';
import { getOperacoes, getFaturamentos } from './services/apiService';

// Importa TODOS os nossos componentes filhos
import FormularioCadastro from './components/FormularioCadastro.vue';
import OperacoesLista from './components/OperacoesLista.vue';
import FaturamentosLista from './components/FaturamentosLista.vue';
import PainelAnalise from './components/PainelAnalise.vue'; // <-- O NOVO COMPONENTE

// Estado central da aplicação (os dados que controlam tudo)
const operacoes = ref<Operacao[]>([]);
const faturamentos = ref<Faturamento[]>([]);
const erroApi = ref<string | null>(null);
const carregando = ref(true);

// Função para carregar todos os dados da API
async function carregarDados() {
  try {
    const [operacoesData, faturamentosData] = await Promise.all([
      getOperacoes(),
      getFaturamentos()
    ]);
    operacoes.value = operacoesData;
    faturamentos.value = faturamentosData;
  } catch (error) {
    console.error('Falha ao carregar dados:', error);
    erroApi.value = 'Não foi possível carregar os dados da API.';
  } finally {
    carregando.value = false;
  }
}

onMounted(carregarDados);

function handleFaturamentoCadastrado() {
  getFaturamentos().then(data => {
    faturamentos.value = data;
  });
}
</script>

<template>
  <div class="bg-gradient-to-br from-gray-50 to-slate-200 min-h-screen p-4 sm:p-8 font-sans">
    <div class="max-w-4xl mx-auto space-y-8">
      
      <header class="text-center">
        <h1 class="text-4xl font-extrabold text-slate-800">Painel de Faturamento</h1>
        <p class="text-slate-600 mt-2">Uma aplicação Full-Stack com .NET, Vue e Microserviço Node.js.</p>
      </header>
      
      <div v-if="carregando" class="text-center py-8">
        <p class="text-lg text-gray-500 animate-pulse">Carregando dados da API...</p>
      </div>
      <div v-else-if="erroApi" class="bg-red-100 border-l-4 border-red-500 text-red-700 p-4 rounded">
        <p class="font-bold">Erro de Conexão</p>
        <p>{{ erroApi }}</p>
      </div>

      <!-- Aqui o "chefe" (App.vue) usa todos os seus "trabalhadores" -->
      <div v-else class="space-y-8">
        <FormularioCadastro 
          :operacoes="operacoes" 
          @faturamento-cadastrado="handleFaturamentoCadastrado" 
        />
        
        <!-- O NOSSO NOVO PAINEL EM AÇÃO -->
        <!-- Passamos a lista de operações para que ele possa preencher o seu dropdown -->
        <PainelAnalise :operacoes="operacoes" />

        <FaturamentosLista :faturamentos="faturamentos" />
        <OperacoesLista :operacoes="operacoes" />
      </div>

    </div>
  </div>
</template>
