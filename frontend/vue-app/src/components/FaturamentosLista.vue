<script setup lang="ts">
// Define as "props" que o componente espera receber do componente pai (App.vue).
defineProps<{
  faturamentos: {
    id: number,
    data: string,
    valor: number,
    operacao: {
      nome: string
    } | null
  }[]
}>();

// Função auxiliar para formatar números como moeda (R$).
function formatarMoeda(valor: number) {
  return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}
</script>

<template>
  <div class="bg-white p-6 rounded-xl shadow-lg transition-shadow hover:shadow-2xl">
    <h2 class="text-2xl font-bold text-gray-800 mb-4 border-b pb-2">Últimos Lançamentos</h2>
    
    <div v-if="!faturamentos.length" class="text-center py-8 text-gray-500">
      <p>Nenhum faturamento cadastrado ainda.</p>
      <p class="text-sm">Use o formulário acima para adicionar o primeiro!</p>
    </div>

    <div v-else class="overflow-x-auto">
      <table class="min-w-full bg-white">
        <thead class="bg-gray-800 text-white">
          <tr>
            <th class="py-3 px-4 uppercase font-semibold text-sm text-left">Operação</th>
            <th class="py-3 px-4 uppercase font-semibold text-sm text-left">Data</th>
            <th class="py-3 px-4 uppercase font-semibold text-sm text-right">Valor</th>
          </tr>
        </thead>
        <tbody class="text-gray-700">
          <tr v-for="fat in faturamentos" :key="fat.id" class="border-b hover:bg-gray-50 transition-colors duration-200">
            <td class="py-3 px-4 font-medium">{{ fat.operacao?.nome ?? 'N/A' }}</td>
            <td class="py-3 px-4">{{ new Date(fat.data).toLocaleDateString('pt-BR') }}</td>
            <td class="py-3 px-4 text-right font-mono text-indigo-600">{{ formatarMoeda(fat.valor) }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>