import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
// Importa o nosso novo router
import router from './router'

const app = createApp(App)

// Diz à aplicação para usar o sistema de rotas
app.use(router)

app.mount('#app')

