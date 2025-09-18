import axios from 'axios';
// Importa todos os "dicionários" de tipo de que precisamos
import type { Operacao, Faturamento, ResultadoAnalise, ResultadoPrevisao, RespostaPaginada, ProgressoMeta } from '../types';

// A URL base da nossa API .NET. Se a porta mudar, só precisamos de alterar aqui.
const API_URL = 'http://localhost:5013/api';

const apiClient = axios.create({
  baseURL: API_URL,
});

// --- CRUD DE OPERAÇÕES ---
// Define a "forma" dos dados para criar/atualizar uma operação
export interface SalvarOperacaoPayload {
  nome: string;
  descricao: string | null;
  metaMensal: number;
}

export async function getOperacoes(): Promise<Operacao[]> {
  const response = await apiClient.get('/operacoes');
  return response.data;
}

export async function createOperacao(payload: SalvarOperacaoPayload): Promise<Operacao> {
  const response = await apiClient.post('/operacoes', payload);
  return response.data;
}

export async function updateOperacao(id: number, payload: SalvarOperacaoPayload): Promise<void> {
  await apiClient.put(`/operacoes/${id}`, payload);
}

export async function deleteOperacao(id: number): Promise<void> {
  await apiClient.delete(`/operacoes/${id}`);
}


// --- Funções de Faturamentos ---
export interface FiltrosFaturamento {
  operacaoId?: number | null;
  dataInicio?: string | null;
  dataFim?: string | null;
  pagina?: number;
  tamanhoPagina?: number;
}

export async function getFaturamentos(filtros: FiltrosFaturamento): Promise<RespostaPaginada<Faturamento>> {
  const params = new URLSearchParams();
  if (filtros.operacaoId) params.append('operacaoId', String(filtros.operacaoId));
  if (filtros.dataInicio) params.append('dataInicio', filtros.dataInicio);
  if (filtros.dataFim) params.append('dataFim', filtros.dataFim);
  if (filtros.pagina) params.append('pagina', String(filtros.pagina));
  if (filtros.tamanhoPagina) params.append('tamanhoPagina', String(filtros.tamanhoPagina));
  const response = await apiClient.get('/faturamentos', { params });
  return response.data;
}

export async function createFaturamento(data: { operacaoId: number; dataRef: string; valor: number }): Promise<Faturamento> {
  const response = await apiClient.post('/faturamentos', data);
  return response.data;
}

// --- Funções de Análise ---
export async function getProgressoMeta(operacaoId: number): Promise<ProgressoMeta> {
    const response = await apiClient.get(`/analysis/progresso-meta/${operacaoId}`);
    return response.data;
}

export async function getMediaDiaria(operacaoId: number): Promise<ResultadoAnalise> {
  const response = await apiClient.get(`/analysis/daily-average/${operacaoId}`);
  return response.data;
}

export async function getPrevisaoFaturamento(operacaoId: number): Promise<ResultadoPrevisao> {
  const response = await apiClient.get(`/analysis/forecast/${operacaoId}`);
  return response.data;
}

