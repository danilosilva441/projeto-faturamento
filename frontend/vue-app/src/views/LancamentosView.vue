<script setup lang="ts">
import { ref, onMounted, watch } from 'vue';
// Importa as ferramentas que vamos usar
import type { Faturamento, Operacao, RespostaPaginada } from '../types';
import { getFaturamentos, getOperacoes } from '../services/apiService';

// --- ESTADO DA PÁGINA ---

// 1. CORREÇÃO: Criamos uma interface dedicada para a paginação. É mais limpo que 'Omit'.
interface PaginacaoState {
  totalItems: number;
  paginaAtual: number;
  totalPaginas: number;
}

const faturamentos = ref<Faturamento[]>([]);
const operacoes = ref<Operacao[]>([]);
const paginacaoInfo = ref<PaginacaoState>({
  totalItems: 0,
  paginaAtual: 1,
  totalPaginas: 1,
});
const carregando = ref(true);
const erro = ref<string | null>(null);

// Define a "forma" exata do nosso objeto de filtros
interface FiltrosState {
  operacaoId: number | null;
  dataInicio: string | null;
  dataFim: string | null;
  pagina: number;
}

// Usa esta "forma" (FiltrosState) para tipar o nosso 'ref' corretamente.
const filtros = ref<FiltrosState>({
  operacaoId: null,
  dataInicio: null,
  dataFim: null,
  pagina: 1,
});

// --- LÓGICA ---
async function buscarLancamentos() {
  carregando.value = true;
  erro.value = null;
  try {
    const dados = await getFaturamentos({
      ...filtros.value,
      tamanhoPagina: 15, // Define quantos itens por página
    });
    faturamentos.value = dados.items;
    paginacaoInfo.value = {
      totalItems: dados.totalItems,
      paginaAtual: dados.paginaAtual,
      totalPaginas: dados.totalPaginas,
    };
  } catch (e) {
    console.error('Erro ao buscar lançamentos:', e);
    erro.value = 'Não foi possível carregar os lançamentos.';
  } finally {
    carregando.value = false;
  }
}

function mudarPagina(novaPagina: number) {
  if (novaPagina >= 1 && novaPagina <= paginacaoInfo.value.totalPaginas) {
    filtros.value.pagina = novaPagina;
  }
}

function limparFiltros() {
  filtros.value = {
    operacaoId: null,
    dataInicio: null,
    dataFim: null,
    pagina: 1,
  };
}

// "Vigia" os filtros. Se algo mudar, busca os dados novamente.
watch(filtros, buscarLancamentos, { deep: true });


// 2. CORREÇÃO: O 'onMounted' agora usa apenas async/await, tornando o código mais limpo.
onMounted(async () => {
    try {
        // Busca as operações para o dropdown
        operacoes.value = await getOperacoes();
        // E busca a primeira página de lançamentos
        await buscarLancamentos();
    } catch (e) {
        erro.value = 'Não foi possível carregar os dados iniciais da página.';
        console.error(e);
    }
});

function formatarMoeda(valor: number) {
  return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}
</script>

<template>
  <div class="space-y-6">
    <!-- Cabeçalho da Página -->
    <h1 class="text-3xl font-bold text-gray-800">Histórico de Lançamentos</h1>

    <!-- Card de Filtros -->
    <div class="bg-white p-4 rounded-lg shadow-md">
      <div class="grid grid-cols-1 md:grid-cols-4 gap-4 items-end">
        <!-- Filtro de Operação -->
        <div>
          <label for="filtro-operacao" class="block text-sm font-medium text-gray-700">Operação</label>
          <select id="filtro-operacao" v-model="filtros.operacaoId" class="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm">
            <option :value="null">Todas</option>
            <option v-for="op in operacoes" :key="op.id" :value="op.id">{{ op.nome }}</option>
          </select>
        </div>
        <!-- Filtro de Data Início -->
        <div>
          <label for="filtro-data-inicio" class="block text-sm font-medium text-gray-700">De</label>
          <input type="date" id="filtro-data-inicio" v-model="filtros.dataInicio" class="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm" />
        </div>
        <!-- Filtro de Data Fim -->
        <div>
          <label for="filtro-data-fim" class="block text-sm font-medium text-gray-700">Até</label>
          <input type="date" id="filtro-data-fim" v-model="filtros.dataFim" class="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm" />
        </div>
        <!-- Botão Limpar -->
        <button @click="limparFiltros" class="py-2 px-4 border rounded-md text-sm font-medium text-gray-700 bg-gray-100 hover:bg-gray-200">Limpar Filtros</button>
      </div>
    </div>

    <!-- Tabela de Resultados -->
    <div class="bg-white p-6 rounded-lg shadow-md">
      <div v-if="carregando" class="text-center py-8 text-gray-500">
        A carregar lançamentos...
      </div>
      <div v-else-if="erro" class="text-center py-8 text-red-500">
        {{ erro }}
      </div>
      <div v-else-if="!faturamentos.length" class="text-center py-8 text-gray-500">
        Nenhum lançamento encontrado para os filtros selecionados.
      </div>
      <div v-else>
        <!-- Tabela -->
        <div class="overflow-x-auto">
            <table class="min-w-full bg-white">
                <thead class="bg-gray-800 text-white">
                <tr>
                    <th class="py-3 px-4 uppercase font-semibold text-sm text-left">Operação</th>
                    <th class="py-3 px-4 uppercase font-semibold text-sm text-left">Data</th>
                    <th class="py-3 px-4 uppercase font-semibold text-sm text-right">Valor</th>
                </tr>
                </thead>
                <tbody class="text-gray-700">
                <tr v-for="fat in faturamentos" :key="fat.id" class="border-b hover:bg-gray-50">
                    <td class="py-3 px-4">{{ fat.operacao?.nome ?? 'N/A' }}</td>
                    <td class="py-3 px-4">{{ new Date(fat.data).toLocaleDateString('pt-BR') }}</td>
                    <td class="py-3 px-4 text-right font-mono">{{ formatarMoeda(fat.valor) }}</td>
                </tr>
                </tbody>
            </table>
        </div>
        <!-- Controles de Paginação -->
        <div class="mt-4 flex justify-between items-center text-sm text-gray-600">
            <span>A exibir {{ faturamentos.length }} de {{ paginacaoInfo.totalItems }} registos</span>
            <div class="flex gap-2">
                <button @click="mudarPagina(paginacaoInfo.paginaAtual - 1)" :disabled="paginacaoInfo.paginaAtual <= 1" class="px-3 py-1 border rounded disabled:opacity-50">Anterior</button>
                <span>Página {{ paginacaoInfo.paginaAtual }} de {{ paginacaoInfo.totalPaginas }}</span>
                <button @click="mudarPagina(paginacaoInfo.paginaAtual + 1)" :disabled="paginacaoInfo.paginaAtual >= paginacaoInfo.totalPaginas" class="px-3 py-1 border rounded disabled:opacity-50">Próxima</button>
            </div>
        </div>
      </div>
    </div>
  </div>
</template>

