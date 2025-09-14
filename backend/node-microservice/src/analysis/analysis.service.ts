import { Injectable } from '@nestjs/common';

// As "receitas" (interfaces) para os nossos dados, marcadas como públicas
export interface HistoricoItem {
  data: string;
  valor: number;
}

export interface PrevisaoItem {
  data: string;
  valorPrevisto: number;
}

@Injectable()
export class AnalysisService {
  
  // A nossa função para calcular a média
  calculateAverage(valores: number[]): number {
    if (valores.length === 0) return 0;
    const sum = valores.reduce((acc, val) => acc + val, 0);
    return parseFloat((sum / valores.length).toFixed(2));
  }

  // A nossa função para gerar a previsão
  generateForecast(historico: HistoricoItem[]): PrevisaoItem[] {
    // 1. Agrupar faturamentos por dia da semana
    const mediaPorDiaDaSemana: { [key: number]: { soma: number; contagem: number } } = {};
    for (const item of historico) {
      const data = new Date(item.data);
      const diaDaSemana = data.getUTCDay();

      if (!mediaPorDiaDaSemana[diaDaSemana]) {
        mediaPorDiaDaSemana[diaDaSemana] = { soma: 0, contagem: 0 };
      }
      mediaPorDiaDaSemana[diaDaSemana].soma += item.valor;
      mediaPorDiaDaSemana[diaDaSemana].contagem++;
    }

    // 2. Calcular a média para cada dia da semana
    const baselineDiario: { [key: number]: number } = {};
    for (const dia in mediaPorDiaDaSemana) {
      const media = mediaPorDiaDaSemana[dia].soma / mediaPorDiaDaSemana[dia].contagem;
      baselineDiario[dia] = parseFloat(media.toFixed(2));
    }
    
    // 3. Gerar a previsão para os próximos 7 dias
    const previsao: PrevisaoItem[] = [];
    const hoje = new Date();
    for (let i = 1; i <= 7; i++) {
      const dataFutura = new Date(hoje);
      dataFutura.setUTCDate(hoje.getUTCDate() + i);
      const diaDaSemanaFuturo = dataFutura.getUTCDay();
      const valorPrevisto = baselineDiario[diaDaSemanaFuturo] || 0;
      previsao.push({
        data: dataFutura.toISOString().split('T')[0],
        valorPrevisto: valorPrevisto,
      });
    }
    return previsao;
  }
}

