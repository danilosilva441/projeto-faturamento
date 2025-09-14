import { Controller, Post, Body, ValidationPipe } from '@nestjs/common';
import { AnalysisService } from './analysis.service';
import { CalculateAverageDto } from './dto/calculate-average.dto';
import { ForecastDto } from './dto/forecast.dto'; // Importa o nosso novo DTO

@Controller('analysis')
export class AnalysisController {
  constructor(private readonly analysisService: AnalysisService) {}

  // O endpoint de média que já tínhamos
  @Post('average')
  calculateAverage(@Body(new ValidationPipe()) data: CalculateAverageDto) {
    const average = this.analysisService.calculateAverage(data.valores);
    return {
      mediaCalculada: average,
    };
  }

  // --- NOVO ENDPOINT DE PREVISÃO ---
  @Post('forecast') // Rota: POST /analysis/forecast
  generateForecast(@Body(new ValidationPipe()) data: ForecastDto) {
    // Chama o serviço com os dados históricos recebidos
    const forecast = this.analysisService.generateForecast(data.historico);
    // Retorna a previsão
    return {
      previsao: forecast,
    };
  }
}
