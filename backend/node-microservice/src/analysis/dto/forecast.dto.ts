import { Type } from 'class-transformer';
import { IsArray, IsDateString, IsNotEmpty, IsNumber, ValidateNested } from 'class-validator';

// Define a "forma" de cada item individual nos dados históricos
class HistoricoItemDto {
  @IsDateString() // Garante que é uma data no formato string (ex: "2025-09-15")
  data: string;

  @IsNumber()
  valor: number;
}

// Define a "forma" do corpo (body) completo que a requisição deve ter
export class ForecastDto {
  @IsArray()
  @IsNotEmpty()
  @ValidateNested({ each: true }) // Diz ao Nest para validar cada item do array usando as regras do HistoricoItemDto
  @Type(() => HistoricoItemDto) // Ajuda o Nest a converter os objetos recebidos para a nossa classe
  historico: HistoricoItemDto[];
}
