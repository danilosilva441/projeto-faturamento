export interface Operacao {
  id: number;
  nome: string;
  descricao: string | null;
  ativo: boolean;
  criadoEm: string;
}

export interface Faturamento {
  id: number;
  data: string;
  valor: number;
  operacao: Operacao | null;
}

// --- NOVOS TIPOS PARA ANÁLISE E PREVISÃO ---
// Define a "forma" da resposta do endpoint de média
export interface ResultadoAnalise {
  mediaCalculada: number;
}

// Define a "forma" de cada item individual na lista de previsão
export interface PrevisaoItem {
  data: string;
  valorPrevisto: number;
}

// Define a "forma" da resposta completa do endpoint de previsão
export interface ResultadoPrevisao {
  previsao: PrevisaoItem[];
}