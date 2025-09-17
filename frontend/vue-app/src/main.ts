import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
// Importa o nosso novo sistema de rotas
import router from './router'

const app = createApp(App)

// Diz à aplicação para usar o sistema de rotas que importámos
app.use(router)

// Monta a aplicação no ecrã
app.mount('#app')

