import { createRouter, createWebHistory } from 'vue-router';
// Importa os componentes que representarão as nossas páginas
import HomeView from '../views/HomeView.vue';
import LancamentosView from '../views/LancamentosView.vue';
import OperacoesView from '../views/OperacoesView.vue';

// Define as rotas (os "endereços" da nossa aplicação)
const routes = [
  {
    path: '/', // O endereço principal (ex: http://localhost:5173/)
    name: 'Home',
    component: HomeView, // Quando o utilizador estiver neste endereço, mostre o HomeView
  },
  {
    path: '/lancamentos', // O endereço da lista de lançamentos
    name: 'Lancamentos',
    component: LancamentosView,
  },
  {
    path: '/operacoes', // O novo endereço para gestão de operações
    name: 'Operacoes',
    component: OperacoesView,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;

