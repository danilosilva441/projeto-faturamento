<script setup lang="ts">
import { ref, onMounted } from 'vue';
import axios from 'axios';

// 1. Importa os novos componentes que criamos
import FormularioCadastro from './components/FormularioCadastro.vue';
import OperacoesLista from './components/OperacoesLista.vue';

// --- DEFINIÇÕES E ESTADO PRINCIPAL ---
// Apenas o App.vue se preocupa com a URL da API e o estado geral (carregando, erro)
const API_URL = 'http://localhost:5013/api';

interface Operacao {
  id: number;
  nome: string;
  descricao: string | null;
  ativo: boolean;
  criadoEm: string;
}

const operacoes = ref<Operacao[]>([]);
const erroApi = ref<string | null>(null);
const carregando = ref<boolean>(true);

// --- FUNÇÃO PARA BUSCAR OS DADOS ---
// A responsabilidade de buscar dados é do componente pai
async function buscarOperacoes() {
  try {
    erroApi.value = null; 
    carregando.value = true;
    const response = await axios.get(`${API_URL}/operacoes`);
    operacoes.value = response.data;
  } catch (error) {
    console.error('Falha ao buscar operações:', error);
    erroApi.value = 'Não foi possível carregar os dados. Verifique se a API backend está rodando.';
  } finally {
    carregando.value = false;
  }
}

// Quando o App.vue é montado na tela, ele busca os dados iniciais
onMounted(buscarOperacoes);

// --- FUNÇÃO PARA LIDAR COM O EVENTO DO FILHO ---
// Esta função é chamada quando o FormularioCadastro emite o evento 'faturamentoCadastrado'
function handleFaturamentoCadastrado(novoFaturamento: any) {
  console.log('Evento recebido no App.vue! Novo faturamento:', novoFaturamento);
  // No futuro, podemos adicionar o novo faturamento a uma lista de "Últimos Lançamentos" aqui,
  // ou simplesmente recarregar todos os dados.
}

</script>

<template>
  <!-- O App.vue agora define o layout GERAL da página -->
  <div class="bg-gradient-to-br from-gray-50 to-slate-200 min-h-screen p-4 sm:p-8 font-sans">
    <div class="max-w-4xl mx-auto space-y-8">
      
      <header class="text-center">
        <h1 class="text-4xl font-extrabold text-slate-800">Painel de Faturamento</h1>
        <p class="text-slate-600 mt-2">Uma aplicação Full-Stack com .NET, Vue e Docker.</p>
      </header>
      
      <!-- Mensagens Globais de Carregamento e Erro -->
      <div v-if="carregando" class="text-center py-8">
        <p class="text-lg text-gray-500 animate-pulse">Carregando dados da API...</p>
      </div>
      <div v-else-if="erroApi" class="bg-red-100 border-l-4 border-red-500 text-red-700 p-4 rounded">
        <p class="font-bold">Erro de Conexão</p>
        <p>{{ erroApi }}</p>
      </div>

      <!-- Usando os Componentes (se não houver erro de carregamento) -->
      <div v-else class="space-y-8">
        
        <!-- O componente do formulário -->
        <!-- :operacoes="operacoes" -> Passa a lista de operações como "prop" para o filho -->
        <!-- @faturamento-cadastrado -> "Ouve" o evento do filho e chama nossa função handle... -->
        <FormularioCadastro 
          :operacoes="operacoes" 
          @faturamento-cadastrado="handleFaturamentoCadastrado" 
        />

        <!-- O componente da lista -->
        <!-- :operacoes="operacoes" -> Também passa a lista para o componente da tabela -->
        <OperacoesLista :operacoes="operacoes" />
      </div>

    </div>
  </div>
</template>

