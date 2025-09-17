<script setup lang="ts">
import { ref, onMounted } from 'vue';
// Importa os tipos e as funções de serviço que esta página precisa
import type { Operacao, Faturamento } from '../types';
import { getOperacoes, getFaturamentos } from '../services/apiService';

// Importa os componentes "filhos" que esta página usa
import FormularioCadastro from '../components/FormularioCadastro.vue';
import OperacoesLista from '../components/OperacoesLista.vue';
import FaturamentosLista from '../components/FaturamentosLista.vue';
import PainelAnalise from '../components/PainelAnalise.vue';

// --- ESTADO DA PÁGINA ---
const operacoes = ref<Operacao[]>([]);
const faturamentos = ref<Faturamento[]>([]);
const erroApi = ref<string | null>(null);
const carregando = ref(true);

// --- LÓGICA DE DADOS (CORRIGIDA E SIMPLIFICADA) ---
// Função para carregar todos os dados necessários para a página
async function carregarDados() {
  carregando.value = true;
  erroApi.value = null;
  try {
    // Busca as operações e os faturamentos iniciais em paralelo para ser mais rápido
    // e de forma mais limpa, usando apenas async/await.
    const [operacoesData, faturamentosPaginadosData] = await Promise.all([
      getOperacoes(),
      getFaturamentos({ pagina: 1, tamanhoPagina: 10 }) // Busca a primeira página de faturamentos
    ]);

    // Atribui os resultados às nossas variáveis de estado
    operacoes.value = operacoesData;
    faturamentos.value = faturamentosPaginadosData.items;

  } catch (error) {
    console.error('Falha ao carregar dados:', error);
    erroApi.value = 'Não foi possível carregar os dados da API.';
  } finally {
    carregando.value = false;
  }
}

// Carrega os dados assim que a página é montada no ecrã
onMounted(carregarDados);

// Função para recarregar a lista de faturamentos após um novo cadastro
async function handleFaturamentoCadastrado() {
  console.log('Novo faturamento cadastrado! A recarregar a lista...');
  try {
    const respostaPaginada = await getFaturamentos({ pagina: 1, tamanhoPagina: 10 });
    faturamentos.value = respostaPaginada.items;
  } catch(error) {
    console.error('Falha ao recarregar faturamentos:', error);
  }
}
</script>

<template>
  <!-- O layout da página principal (dashboard) não sofreu alterações -->
  <div class="space-y-8">
    
    <div v-if="carregando" class="text-center py-8">
      <p class="text-lg text-gray-500 animate-pulse">A carregar dados da API...</p>
    </div>
    <div v-else-if="erroApi" class="bg-red-100 border-l-4 border-red-500 text-red-700 p-4 rounded">
      <p class="font-bold">Erro de Conexão</p>
      <p>{{ erroApi }}</p>
    </div>

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

