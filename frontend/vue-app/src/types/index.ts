export interface Operacao {
  id: number;
  nome: string;
  descricao: string | null;
  ativo: boolean;
  criadoEm: string;
  metaMensal: number; // A nova meta mensal
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

export interface ProgressoMeta {
  operacaoNome: string;
  metaMensal: number;
  totalFaturado: number;
  progressoPercentual: number;
  valorRestante: number;
  mesReferencia: string;
}

export interface RespostaPaginada<T> {
  totalItems: number;
  paginaAtual: number;
  totalPaginas: number;
  items: T[];
}

