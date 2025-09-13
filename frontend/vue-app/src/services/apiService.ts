import axios from 'axios';
import type { Operacao, Faturamento } from '../types'; // Importa as nossas definições

// URL base da nossa API. Se mudarmos a porta, só precisamos de alterar aqui.
const API_URL = 'http://localhost:5013/api';

const apiClient = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Função para buscar a lista de operações
export async function getOperacoes(): Promise<Operacao[]> {
  const response = await apiClient.get('/operacoes');
  return response.data.$values; // Já desempacota o resultado aqui
}

// Função para buscar a lista de faturamentos
export async function getFaturamentos(): Promise<Faturamento[]> {
  const response = await apiClient.get('/faturamentos');
  return response.data.$values;
}

// Função para criar um novo faturamento
export async function createFaturamento(data: { operacaoId: number; dataRef: string; valor: number }): Promise<Faturamento> {
  const response = await apiClient.post('/faturamentos', data);
  return response.data;
}
