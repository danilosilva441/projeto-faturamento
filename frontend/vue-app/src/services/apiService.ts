import axios from 'axios';
// Importa todos os tipos, incluindo os novos de previsão, do seu ficheiro "Tipos (com Previsão)"
import type { Operacao, Faturamento, ResultadoAnalise, ResultadoPrevisao } from '../types';

const API_URL = 'http://localhost:5013/api';

const apiClient = axios.create({
  baseURL: API_URL,
});

export async function getOperacoes(): Promise<Operacao[]> {
  const response = await apiClient.get('/operacoes');
  return response.data;
}

export async function getFaturamentos(): Promise<Faturamento[]> {
  const response = await apiClient.get('/faturamentos');
  return response.data;
}

export async function createFaturamento(data: { operacaoId: number; dataRef: string; valor: number }): Promise<Faturamento> {
  const response = await apiClient.post('/faturamentos', data);
  return response.data;
}

export async function getMediaDiaria(operacaoId: number): Promise<ResultadoAnalise> {
  const response = await apiClient.get(`/analysis/daily-average/${operacaoId}`);
  return response.data;
}

// --- NOVA FUNÇÃO DE PREVISÃO ---
// Esta é a nova função que vai chamar o endpoint de previsão do backend .NET
export async function getPrevisaoFaturamento(operacaoId: number): Promise<ResultadoPrevisao> {
  const response = await apiClient.get(`/analysis/forecast/${operacaoId}`);
  return response.data;
}

