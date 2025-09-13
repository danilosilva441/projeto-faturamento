import { createApp } from 'vue'
import './style.css' // Importa o nosso CSS com Tailwind
import App from './App.vue' // Importa o nosso componente principal

// Esta linha é a mais importante:
// Ela cria a aplicação Vue e diz a ela para "montar" (renderizar)
// o componente 'App' dentro do elemento HTML que tem o id="app" no ficheiro index.html.
createApp(App).mount('#app')

