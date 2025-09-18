import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
// Importa o nosso novo sistema de rotas
import router from './router'

createApp(App).use(router).mount('#app')
