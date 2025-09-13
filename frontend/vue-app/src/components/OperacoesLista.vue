<script setup lang="ts">
import type { Operacao } from '../types';

// Define que este componente espera receber uma propriedade chamada 'operacoes',
// que deve ser uma lista de objetos do tipo Operacao.
defineProps<{
  operacoes: Operacao[]
}>();
</script>

<template>
  <div class="bg-white p-6 rounded-xl shadow-lg transition-shadow hover:shadow-2xl">
    <h2 class="text-2xl font-bold text-gray-800 mb-4 border-b pb-2">Operações Cadastradas</h2>
    <div class="overflow-x-auto">
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
</template>

