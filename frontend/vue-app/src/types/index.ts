// Este ficheiro serve como um "dicionário" para o nosso projeto.
// Ele diz ao TypeScript exatamente como são os objetos que recebemos da nossa API .NET.

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
  // Dizemos que um faturamento TEM um objeto Operacao dentro dele (pode ser nulo)
  operacao: Operacao | null;
}
