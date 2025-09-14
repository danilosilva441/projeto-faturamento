    import { Injectable } from '@nestjs/common';

    @Injectable()
    export class AnalysisService {
      calculateAverage(valores: number[]): number {
        if (valores.length === 0) {
          return 0;
        }
        const sum = valores.reduce((acc, val) => acc + val, 0);
        const average = sum / valores.length;
        // Arredonda para 2 casas decimais
        return parseFloat(average.toFixed(2));
      }
    }