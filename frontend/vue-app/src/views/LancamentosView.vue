<script setup lang="ts">
import { ref, onMounted, watch } from 'vue';
// Importa as ferramentas que vamos usar
import type { Faturamento, Operacao, RespostaPaginada } from '../types';
import { getFaturamentos, getOperacoes } from '../services/apiService';

// --- ESTADO DA PÁGINA ---

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

interface FiltrosState {
  operacaoId: number | null;
  dataInicio: string | null;
  dataFim: string | null;
  pagina: number;
}

const filtros = ref<FiltrosState>({
  operacaoId: null,
  dataInicio: null,
  dataFim: null,
  pagina: 1,
});

// --- LÓGICA ---
async function buscarLancamentos() {
  // A função buscarLancamentos agora define 'carregando = true' no início,
  // mas o 'finally' só será executado quando a função terminar.
  // Vamos garantir que o estado de carregamento global só é desligado no final.
  erro.value = null;
  try {
    const dados = await getFaturamentos({
      ...filtros.value,
      tamanhoPagina: 15,
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

// "Vigia" os filtros. Se qualquer valor dentro dele mudar (exceto a página inicial),
// volta para a primeira página e depois o 'watch' chama o buscarLancamentos.
watch(() => ({...filtros.value, pagina: undefined}), () => {
  filtros.value.pagina = 1;
});
watch(filtros, buscarLancamentos, { deep: true });


// --- onMounted (REFATORADO) ---
// Esta versão usa apenas async/await, tornando o código mais limpo e previsível.
onMounted(async () => {
    carregando.value = true;
    try {
        // Busca as operações e os lançamentos em paralelo
        const [operacoesData] = await Promise.all([
            getOperacoes(),
            buscarLancamentos(), // Esta função já atualiza o seu próprio estado
        ]);
        
        // Atribui o resultado das operações após a conclusão
        operacoes.value = operacoesData;

    } catch (e) {
        erro.value = 'Não foi possível carregar os dados iniciais da página.';
        console.error(e);
    } finally {
        carregando.value = false; // Desliga o carregamento global no final de tudo
    }
});

function formatarMoeda(valor: number) {
  return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}
</script>

<template>
  <div class="space-y-6">
    <h1 class="text-3xl font-bold text-gray-800">Histórico de Lançamentos</h1>
    <div class="bg-white p-4 rounded-lg shadow-md">
      <div class="grid grid-cols-1 md:grid-cols-4 gap-4 items-end">
        <div>
          <label for="filtro-operacao" class="block text-sm font-medium text-gray-700">Operação</label>
          <select id="filtro-operacao" v-model="filtros.operacaoId" class="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm">
            <option :value="null">Todas</option>
            <option v-for="op in operacoes" :key="op.id" :value="op.id">{{ op.nome }}</option>
          </select>
        </div>
        <div>
          <label for="filtro-data-inicio" class="block text-sm font-medium text-gray-700">De</label>
          <input type="date" id="filtro-data-inicio" v-model="filtros.dataInicio" class="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm" />
        </div>
        <div>
          <label for="filtro-data-fim" class="block text-sm font-medium text-gray-700">Até</label>
          <input type="date" id="filtro-data-fim" v-model="filtros.dataFim" class="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm" />
        </div>
        <button @click="limparFiltros" class="py-2 px-4 border rounded-md text-sm font-medium text-gray-700 bg-gray-100 hover:bg-gray-200">Limpar Filtros</button>
      </div>
    </div>
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

