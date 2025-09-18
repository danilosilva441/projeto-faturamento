import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '../views/HomeView.vue';
import LancamentosView from '../views/LancamentosView.vue';
import OperacoesView from '../views/OperacoesView.vue';

const routes = [
  { path: '/', name: 'Home', component: HomeView },
  { path: '/lancamentos', name: 'Lancamentos', component: LancamentosView },
  { path: '/operacoes', name: 'Operacoes', component: OperacoesView },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;

