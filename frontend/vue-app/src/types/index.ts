// Este ficheiro serve como um "dicionário" para o nosso projeto.
export interface Operacao {
  id: number;
  nome: string;
  descricao: string | null;
  ativo: boolean;
  criadoEm: string;
  metaMensal: number;
}

export interface Faturamento {
  id: number;
  data: string;
  valor: number;
  operacao: Operacao | null;
}

export interface ResultadoAnalise {
  mediaCalculada: number;
}

export interface PrevisaoItem {
  data: string;
  valorPrevisto: number;
}

export interface ResultadoPrevisao {
  previsao: PrevisaoItem[];
}

export interface RespostaPaginada<T> {
  totalItems: number;
  paginaAtual: number;
  totalPaginas: number;
  items: T[];
}

// --- NOVA INTERFACE PARA O PROGRESSO DA META ---
export interface ProgressoMeta {
  operacaoNome: string;
  metaMensal: number;
  totalFaturado: number;
  progressoPercentual: number;
  valorRestante: number;
  mesReferencia: string;
}

