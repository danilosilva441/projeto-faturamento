import axios from 'axios';
import type { Operacao, Faturamento } from '../types';

// Define a "forma" da resposta que o nosso endpoint de análise retorna
export interface ResultadoAnalise {
  mediaCalculada: number;
}

const API_URL = 'http://localhost:5013/api';

const apiClient = axios.create({
  baseURL: API_URL,
});

// Função para buscar a lista de operações
export async function getOperacoes(): Promise<Operacao[]> {
  const response = await apiClient.get('/operacoes');
  return response.data;
}

// Função para buscar a lista de faturamentos
export async function getFaturamentos(): Promise<Faturamento[]> {
  const response = await apiClient.get('/faturamentos');
  return response.data;
}

// Função para criar um novo faturamento
export async function createFaturamento(data: { operacaoId: number; dataRef: string; valor: number }): Promise<Faturamento> {
  const response = await apiClient.post('/faturamentos', data);
  return response.data;
}

// --- NOVA FUNÇÃO DE ANÁLISE ---
// Função para pedir o cálculo da média diária para uma operação específica
export async function getMediaDiaria(operacaoId: number): Promise<ResultadoAnalise> {
  const response = await apiClient.get(`/analysis/daily-average/${operacaoId}`);
  return response.data;
}

