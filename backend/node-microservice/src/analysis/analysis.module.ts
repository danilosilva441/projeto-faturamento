import { Module } from '@nestjs/common';
// Importa o Service e o Controller a partir dos seus ficheiros corretos
import { AnalysisService } from './analysis.service';
import { AnalysisController } from './analysis.controller';

@Module({
  // Regista o Controller e o Service para que o NestJS saiba que eles existem
  controllers: [AnalysisController],
  providers: [AnalysisService],
})
export class AnalysisModule {}