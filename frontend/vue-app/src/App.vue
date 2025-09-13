<script setup lang="ts">
// --- LÓGICA (TypeScript) ---
import { ref, onMounted } from 'vue';
import axios from 'axios';

// 1. DEFINA A URL DA SUA API .NET (CONFIRA A PORTA!)
const API_URL = 'http://localhost:5013/api';

// 2. DEFINA A "FORMA" DOS DADOS (INTERFACE)
// Isso ajuda o TypeScript a entender o que estamos recebendo da API
interface Operacao {
  id: number;
  nome: string;
  descricao: string | null; // A descrição pode ser nula
  ativo: boolean;
  criadoEm: string;
}

// 3. CRIE AS VARIÁVEIS "REATIVAS"
// 'ref' do Vue faz com que a tela se atualize automaticamente quando esses valores mudam
const operacoes = ref<Operacao[]>([]); // Uma lista vazia para guardar as operações
const erroApi = ref<string | null>(null); // Um lugar para guardar a mensagem de erro
const carregando = ref<boolean>(true); // Começa como 'true' para mostrar uma mensagem de "Carregando..."

// 4. CRIE A FUNÇÃO PARA BUSCAR OS DADOS
// 'onMounted' é um "gatilho" do Vue que executa esta função assim que a página carrega
onMounted(async () => {
  try {
    const response = await axios.get(`${API_URL}/operacoes`);
    operacoes.value = response.data; // Se der certo, preenche a lista
  } catch (error) {
    console.error('Falha ao buscar operações:', error);
    erroApi.value = 'Não foi possível carregar os dados. Verifique se a API backend está rodando e se o CORS está configurado.';
  } finally {
    // 'finally' sempre executa, dando certo ou errado
    carregando.value = false; // Esconde a mensagem de "Carregando..."
  }
});
</script>

<template>
  <!-- --- VISUAL (HTML com Estilos do Tailwind) --- -->
  <div class="bg-gray-100 min-h-screen p-4 sm:p-8 font-sans">
    <div class="max-w-4xl mx-auto bg-white p-6 rounded-xl shadow-lg">

      <h1 class="text-3xl font-bold text-gray-800 mb-4 border-b pb-2">Painel de Faturamento</h1>
      <p class="text-gray-600 mb-6">Lista de Operações cadastradas consumidas da API .NET.</p>

      <!-- 1. Mensagem de Carregando... (só aparece enquanto 'carregando' for true) -->
      <div v-if="carregando" class="text-center py-8">
        <p class="text-lg text-gray-500 animate-pulse">Carregando dados da API...</p>
      </div>

      <!-- 2. Mensagem de Erro (só aparece se 'erroApi' tiver algum texto) -->
      <div v-else-if="erroApi" class="bg-red-100 border-l-4 border-red-500 text-red-700 p-4 rounded">
        <p class="font-bold">Erro de Conexão</p>
        <p>{{ erroApi }}</p>
      </div>

      <!-- 3. Tabela de Operações (só aparece se não estiver carregando e não houver erro) -->
      <div v-else class="overflow-x-auto">
        <table class="min-w-full bg-white">
          <thead class="bg-gray-800 text-white">
            <tr>
              <th class="py-3 px-4 uppercase font-semibold text-sm text-left">ID</th>
              <th class="py-3 px-4 uppercase font-semibold text-sm text-left">Nome da Operação</th>
              <th class="py-3 px-4 uppercase font-semibold text-sm text-left">Descrição</th>
            </tr>
          </thead>
          <tbody class="text-gray-700">
            <tr v-for="op in operacoes" :key="op.id" class="border-b hover:bg-gray-50 transition-colors duration-200">
              <td class="py-3 px-4">{{ op.id }}</td>
              <td class="py-3 px-4 font-medium">{{ op.nome }}</td>
              <td class="py-3 px-4 text-gray-600">{{ op.descricao ?? 'N/A' }}</td>
            </tr>
          </tbody>
        </table>
      </div>

    </div>
  </div>
</template>