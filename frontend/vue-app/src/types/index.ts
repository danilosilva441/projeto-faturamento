// Este ficheiro serve como um "dicionário" central para todos os tipos do nosso projeto.
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

// --- NOVAS INTERFACES (MOVIDAS PARA AQUI) ---
// Define a "forma" dos dados para criar/atualizar uma operação
export interface SalvarOperacaoPayload {
  nome: string;
  descricao: string | null;
  metaMensal: number;
}

// Define a "forma" dos filtros para a busca de faturamentos
export interface FiltrosFaturamento {
  operacaoId?: number | null;
  dataInicio?: string | null;
  dataFim?: string | null;
  pagina?: number;
  tamanhoPagina?: number;
}

