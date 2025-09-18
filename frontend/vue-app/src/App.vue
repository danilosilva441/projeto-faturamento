

<script setup lang="ts">
// A única coisa que este ficheiro precisa de importar são os componentes do router
import { RouterLink, RouterView } from 'vue-router';
</script>

<template>
  <div class="flex h-screen bg-gray-100 font-sans">
    
    <aside class="w-64 flex-shrink-0 bg-gray-800 text-white flex flex-col">
      <div class="h-16 flex items-center justify-center text-xl font-bold border-b border-gray-700">
        <span>Faturamento App</span>
      </div>
      <nav class="flex-grow p-4 space-y-2">
        <router-link to="/" class="flex items-center px-4 py-2 rounded-lg hover:bg-gray-700 transition-colors" active-class="bg-indigo-600">
          <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"></path></svg>
          <span>Painel Principal</span>
        </router-link>
        <router-link to="/lancamentos" class="flex items-center px-4 py-2 rounded-lg hover:bg-gray-700 transition-colors" active-class="bg-indigo-600">
          <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path></svg>
          <span>Lançamentos</span>
        </router-link>
        <router-link to="/operacoes" class="flex items-center px-4 py-2 rounded-lg hover:bg-gray-700 transition-colors" active-class="bg-indigo-600">
          <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4"></path></svg>
          <span>Operações</span>
        </router-link>
      </nav>
    </aside>

    <!-- 2. Área de Conteúdo Principal -->
    <main class="flex-1 overflow-y-auto p-8">
      <!-- 
        ESTA É A LINHA MÁGICA QUE ESTAVA EM FALTA.
        O Vue Router irá automaticamente renderizar a página correta aqui.
      -->
      <router-view />
    </main>

  </div>
</template>

