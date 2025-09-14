import axios from 'axios';
import type { Operacao, Faturamento } from '../types';

// URL base da nossa API. Se mudarmos a porta, só precisamos de alterar aqui.
const API_URL = 'http://localhost:5013/api';

const apiClient = axios.create({
  baseURL: API_URL,
});

// Agora que o backend envia JSON limpo, a função fica muito mais simples.
// O resultado que queremos já está diretamente em "response.data".
export async function getOperacoes(): Promise<Operacao[]> {
  const response = await apiClient.get('/operacoes');
  return response.data;
}

// O mesmo para os faturamentos.
export async function getFaturamentos(): Promise<Faturamento[]> {
  const response = await apiClient.get('/faturamentos');
  return response.data;
}

// A função de criação já estava correta, mas a mantemos aqui para consistência.
export async function createFaturamento(data: { operacaoId: number; dataRef: string; valor: number }): Promise<Faturamento> {
  const response = await apiClient.post('/faturamentos', data);
  return response.data;
}
