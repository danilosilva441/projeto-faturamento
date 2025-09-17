<script setup lang="ts">
// Importa o componente <router-link> que nos permite criar links de navegação
import { RouterLink, RouterView } from 'vue-router';
</script>

<template>
  <!-- Layout principal da aplicação com Flexbox -->
  <div class="flex h-screen bg-gray-100 font-sans">
    
    <!-- 1. Barra de Navegação Lateral (Sidebar) -->
    <aside class="w-64 flex-shrink-0 bg-gray-800 text-white flex flex-col">
      <div class="h-16 flex items-center justify-center text-xl font-bold border-b border-gray-700">
        <span>Faturamento App</span>
      </div>
      <nav class="flex-grow p-4 space-y-2">
        <!-- 
          <router-link> é o componente do Vue Router para criar links.
          O atributo "to" define para qual rota o link aponta.
          A classe "router-link-exact-active" é adicionada automaticamente pelo Vue Router
          ao link que corresponde à página atual, permitindo-nos estilizá-lo de forma diferente.
        -->
        <router-link 
          to="/" 
          class="flex items-center px-4 py-2 rounded-lg hover:bg-gray-700 transition-colors"
          active-class="bg-indigo-600"
        >
          <!-- Ícone SVG para o link -->
          <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"></path></svg>
          <span>Painel Principal</span>
        </router-link>

        <router-link 
          to="/lancamentos" 
          class="flex items-center px-4 py-2 rounded-lg hover:bg-gray-700 transition-colors"
          active-class="bg-indigo-600"
        >
          <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path></svg>
          <span>Lançamentos</span>
        </router-link>
        
        <router-link 
          to="/metas" 
          class="flex items-center px-4 py-2 rounded-lg hover:bg-gray-700 transition-colors"
          active-class="bg-indigo-600"
        >
          <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z"></path></svg>
          <span>Metas</span>
        </router-link>
      </nav>
    </aside>

    <!-- 2. Área de Conteúdo Principal -->
    <main class="flex-1 overflow-y-auto p-8">
      <!-- 
        <router-view /> é o "espaço reservado" mágico. 
        O Vue Router irá automaticamente renderizar o componente da página atual aqui.
        Se estivermos em "/", ele mostrará a HomeView.
        Se estivermos em "/lancamentos", ele mostrará a LancamentosView.
      -->
      <router-view />
    </main>

  </div>
</template>

<style>
/* Estilos globais podem ser adicionados aqui se necessário */
</style>

