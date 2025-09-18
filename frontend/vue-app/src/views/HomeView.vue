<script setup lang="ts">
import { ref, onMounted } from 'vue';
import type { Operacao, Faturamento } from '../types';
import { getOperacoes, getFaturamentos } from '../services/apiService';
import FormularioCadastro from '../components/FormularioCadastro.vue';
import OperacoesLista from '../components/OperacoesLista.vue';
import FaturamentosLista from '../components/FaturamentosLista.vue';
import PainelAnalise from '../components/PainelAnalise.vue';
import ProgressoMeta from '../components/ProgressoMeta.vue';

const operacoes = ref<Operacao[]>([]);
const faturamentos = ref<Faturamento[]>([]);
const erroApi = ref<string | null>(null);
const carregando = ref(true);

async function carregarDados() {
  carregando.value = true;
  erroApi.value = null;
  try {
    const [operacoesData, faturamentosPaginados] = await Promise.all([
      getOperacoes(),
      getFaturamentos({ pagina: 1, tamanhoPagina: 10 })
    ]);
    operacoes.value = operacoesData;
    faturamentos.value = faturamentosPaginados.items;
  } catch (error) {
    console.error('Falha ao carregar dados da Home:', error);
    erroApi.value = 'Não foi possível carregar os dados do painel principal.';
  } finally {
    carregando.value = false;
  }
}

onMounted(carregarDados);

async function handleFaturamentoCadastrado() {
  try {
    const respostaPaginada = await getFaturamentos({ pagina: 1, tamanhoPagina: 10 });
    faturamentos.value = respostaPaginada.items;
  } catch(error) {
    console.error('Falha ao recarregar faturamentos:', error);
  }
}
</script>

<template>
  <div class="space-y-8">
    <div v-if="carregando" class="text-center py-8">
      <p class="text-lg text-gray-500 animate-pulse">A carregar dados da API...</p>
    </div>
    <div v-else-if="erroApi" class="bg-red-100 border-l-4 border-red-500 text-red-700 p-4 rounded">
      <p class="font-bold">Erro de Conexão</p>
      <p>{{ erroApi }}</p>
    </div>
    <div v-else class="space-y-8">
      <ProgressoMeta :operacoes="operacoes" />
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

