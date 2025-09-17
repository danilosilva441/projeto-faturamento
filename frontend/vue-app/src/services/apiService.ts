import axios from 'axios';
// Importa os nossos tipos, incluindo a nova RespostaPaginada
import type { Faturamento, Operacao, ProgressoMeta, RespostaPaginada, ResultadoAnalise, ResultadoPrevisao } from '../types';

const API_URL = 'http://localhost:5013/api';

const apiClient = axios.create({
  baseURL: API_URL,
});

// --- Funções de Operações e Faturamentos ---
export async function getOperacoes(): Promise<Operacao[]> {
  const response = await apiClient.get('/operacoes');
  return response.data;
}

// Define a "forma" dos filtros que a função pode receber
export interface FiltrosFaturamento {
  operacaoId?: number | null;
  dataInicio?: string | null;
  dataFim?: string | null;
  pagina?: number;
  tamanhoPagina?: number;
}

// A função agora aceita um objeto de filtros e retorna a nossa RespostaPaginada
export async function getFaturamentos(filtros: FiltrosFaturamento): Promise<RespostaPaginada<Faturamento>> {
  // Constrói os parâmetros da URL dinamicamente
  const params = new URLSearchParams();
  if (filtros.operacaoId) params.append('operacaoId', String(filtros.operacaoId));
  if (filtros.dataInicio) params.append('dataInicio', filtros.dataInicio);
  if (filtros.dataFim) params.append('dataFim', filtros.dataFim);
  if (filtros.pagina) params.append('pagina', String(filtros.pagina));
  if (filtros.tamanhoPagina) params.append('tamanhoPagina', String(filtros.tamanhoPagina));

  // Faz a requisição GET, passando os parâmetros
  const response = await apiClient.get('/faturamentos', { params });
  return response.data;
}

export async function createFaturamento(data: { operacaoId: number; dataRef: string; valor: number }): Promise<Faturamento> {
  const response = await apiClient.post('/faturamentos', data);
  return response.data;
}

// --- Funções de Análise ---
export async function getMediaDiaria(operacaoId: number): Promise<ResultadoAnalise> {
  const response = await apiClient.get(`/analysis/daily-average/${operacaoId}`);
  return response.data;
}

export async function getPrevisaoFaturamento(operacaoId: number): Promise<ResultadoPrevisao> {
  const response = await apiClient.get(`/analysis/forecast/${operacaoId}`);
  return response.data;
}

// --- Nova Função de Progresso de Meta ---
export async function getProgressoMeta(operacaoId: number): Promise<ProgressoMeta> {
    const response = await apiClient.get(`/analysis/progresso-meta/${operacaoId}`);
    return response.data;
}

