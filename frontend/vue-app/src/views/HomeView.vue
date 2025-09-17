<script setup lang="ts">
// IMPORTANTE: Nós movemos toda a lógica que estava no App.vue para aqui.
// Esta "View" agora é a responsável por carregar e exibir o painel principal.
import { ref, onMounted } from 'vue';
import type { Operacao, Faturamento } from '../types';
import { getOperacoes, getFaturamentos } from '../services/apiService';

// Importa os componentes "trabalhadores" que esta página usa
import FormularioCadastro from '../components/FormularioCadastro.vue';
import OperacoesLista from '../components/OperacoesLista.vue';
import FaturamentosLista from '../components/FaturamentosLista.vue';
import PainelAnalise from '../components/PainelAnalise.vue';

const operacoes = ref<Operacao[]>([]);
const faturamentos = ref<Faturamento[]>([]);
const erroApi = ref<string | null>(null);
const carregando = ref(true);

async function carregarDados() {
  carregando.value = true;
  try {
    // Busca os dados em paralelo para ser mais rápido
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

// Carrega os dados assim que a página é montada no ecrã
onMounted(carregarDados);

// Função que é chamada quando o formulário avisa que um novo faturamento foi criado
function handleFaturamentoCadastrado() {
  // Apenas busca a lista de faturamentos novamente para mostrar o item novo
  getFaturamentos().then(data => {
    faturamentos.value = data;
  });
}
</script>

<template>
  <!-- Este é o layout da nossa página principal (dashboard) -->
  <div class="space-y-8">
    
    <!-- Mensagens de Carregando e Erro -->
    <div v-if="carregando" class="text-center py-8">
      <p class="text-lg text-gray-500 animate-pulse">A carregar dados da API...</p>
    </div>
    <div v-else-if="erroApi" class="bg-red-100 border-l-4 border-red-500 text-red-700 p-4 rounded">
      <p class="font-bold">Erro de Conexão</p>
      <p>{{ erroApi }}</p>
    </div>

    <!-- Corpo principal da página, que usa os componentes "trabalhadores" -->
    <div v-else class="space-y-8">
      <FormularioCadastro 
        :operacoes="operacoes" 
        @faturamento-cadastrado="handleFaturamentoCadastrado" 
      />
      <PainelAnalise :operacoes="operacoes" />
      <FaturamentosLista :faturamentos="faturamentos" />
      <OperacoesLista :operacoes="operacoes" />
    </div>
    
  </div>
</template>
