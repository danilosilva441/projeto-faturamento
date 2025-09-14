    import { Controller, Post, Body, ValidationPipe } from '@nestjs/common';
    import { AnalysisService } from './analysis.service';
    import { CalculateAverageDto } from './dto/calculate-average.dto';

    @Controller('analysis') // Define a rota base como /analysis
    export class AnalysisController {
      constructor(private readonly analysisService: AnalysisService) {}

      @Post('average') // Rota completa: POST /analysis/average
      // Usa o 'ValidationPipe' para ativar as validações do nosso DTO automaticamente
      calculateAverage(@Body(new ValidationPipe()) data: CalculateAverageDto) {
        const average = this.analysisService.calculateAverage(data.valores);
        return {
          mediaCalculada: average,
        };
      }
    }