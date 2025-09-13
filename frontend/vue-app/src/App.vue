<script setup lang="ts">
import { ref, onMounted } from 'vue';
import axios from 'axios';

// Importa os três componentes que formam a nossa página.
import FormularioCadastro from './components/FormularioCadastro.vue';
import OperacoesLista from './components/OperacoesLista.vue';
import FaturamentosLista from './components/FaturamentosLista.vue';

// --- ESTADO CENTRAL DA APLICAÇÃO ---
const API_URL = 'http://localhost:5013/api';

interface Operacao {
  id: number;
  nome: string;
  descricao: string | null;
}

interface Faturamento {
  id: number;
  data: string;
  valor: number;
  operacao: { nome: string } | null;
}

const operacoes = ref<Operacao[]>([]);
const faturamentos = ref<Faturamento[]>([]); // Nova lista para os faturamentos
const erroApi = ref<string | null>(null);
const carregando = ref<boolean>(true);

// --- FUNÇÕES DE INTERAÇÃO COM A API ---

// Busca a lista de operações
async function buscarOperacoes() {
  const response = await axios.get(`${API_URL}/operacoes`);
  operacoes.value = response.data;
}

// Busca a lista de faturamentos
async function buscarFaturamentos() {
  const response = await axios.get(`${API_URL}/faturamentos`);
  faturamentos.value = response.data;
}

// Ao carregar a página, busca todos os dados necessários em paralelo.
onMounted(async () => {
  try {
    carregando.value = true;
    await Promise.all([
      buscarOperacoes(),
      buscarFaturamentos()
    ]);
  } catch (error) {
    console.error('Falha ao carregar dados iniciais:', error);
    erroApi.value = 'Não foi possível carregar os dados. Verifique se a API backend está rodando.';
  } finally {
    carregando.value = false;
  }
});

// --- ATUALIZAÇÃO EM TEMPO REAL ---
// Esta função é chamada pelo evento do formulário e recarrega a lista de faturamentos.
function handleFaturamentoCadastrado() {
  console.log('Novo faturamento cadastrado! Recarregando a lista...');
  buscarFaturamentos(); 
}

</script>

<template>
  <div class="bg-gradient-to-br from-gray-50 to-slate-200 min-h-screen p-4 sm:p-8 font-sans">
    <div class="max-w-4xl mx-auto space-y-8">
      
      <header class="text-center">
        <h1 class="text-4xl font-extrabold text-slate-800">Painel de Faturamento</h1>
        <p class="text-slate-600 mt-2">Uma aplicação Full-Stack com .NET, Vue e Docker.</p>
      </header>
      
      <div v-if="carregando" class="text-center py-8">
        <p class="text-lg text-gray-500 animate-pulse">Carregando dados da API...</p>
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

        <FaturamentosLista :faturamentos="faturamentos" />

        <OperacoesLista :operacoes="operacoes" />
      </div>

    </div>
  </div>
</template>