<script setup lang="ts">
import { ref, onMounted } from 'vue';
// Importa as definições de tipo e as funções da API que criámos
import type { Operacao, Faturamento } from './types';
import { getOperacoes, getFaturamentos } from './services/apiService';

// Importa os componentes filhos que vamos usar no template
import FormularioCadastro from './components/FormularioCadastro.vue';
import OperacoesLista from './components/OperacoesLista.vue';
import FaturamentosLista from './components/FaturamentosLista.vue';

// Variáveis centrais que guardam os dados da aplicação
const operacoes = ref<Operacao[]>([]);
const faturamentos = ref<Faturamento[]>([]);
const erroApi = ref<string | null>(null);
const carregando = ref(true);

// Função para carregar todos os dados da API quando a página abre
async function carregarDados() {
  try {
    // Busca operações e faturamentos ao mesmo tempo para ser mais rápido
    const [operacoesData, faturamentosData] = await Promise.all([
      getOperacoes(),
      getFaturamentos()
    ]);
    operacoes.value = operacoesData;
    faturamentos.value = faturamentosData;
  } catch (error) {
    console.error('Falha ao carregar dados iniciais:', error);
    erroApi.value = 'Não foi possível carregar os dados da API.';
  } finally {
    carregando.value = false;
  }
}

// Roda a função acima assim que a página é montada
onMounted(carregarDados);

// Função que é chamada QUANDO o FormularioCadastro nos avisa que um item foi criado
function handleFaturamentoCadastrado() {
  // Apenas recarrega a lista de faturamentos para mostrar o novo item
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
        <p class="text-slate-600 mt-2">Uma aplicação Full-Stack com .NET, Vue e Docker.</p>
      </header>
      
      <!-- Mensagens de Carregando e Erro -->
      <div v-if="carregando" class="text-center py-8">
        <p class="text-lg text-gray-500 animate-pulse">Carregando dados da API...</p>
      </div>
      <div v-else-if="erroApi" class="bg-red-100 border-l-4 border-red-500 text-red-700 p-4 rounded">
        <p class="font-bold">Erro de Conexão</p>
        <p>{{ erroApi }}</p>
      </div>

      <!-- Corpo principal da aplicação, onde os componentes são usados -->
      <div v-else class="space-y-8">
        <!-- 
          Componente do Formulário 
          :operacoes="operacoes" -> "Filho, aqui está a lista de operações que você precisa."
          @faturamento-cadastrado -> "Filho, quando você terminar de cadastrar, me avise chamando a função 'handleFaturamentoCadastrado'."
        -->
        <FormularioCadastro 
          :operacoes="operacoes" 
          @faturamento-cadastrado="handleFaturamentoCadastrado" 
        />
        <!-- Componente da Lista de Faturamentos -->
        <FaturamentosLista :faturamentos="faturamentos" />
        <!-- Componente da Lista de Operações -->
        <OperacoesLista :operacoes="operacoes" />
      </div>

    </div>
  </div>
</template>

