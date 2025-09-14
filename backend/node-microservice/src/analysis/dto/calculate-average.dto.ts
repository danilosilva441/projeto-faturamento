    // Importa as ferramentas de validação do NestJS
    import { IsArray, IsNotEmpty, IsNumber } from 'class-validator';

    export class CalculateAverageDto {
      @IsArray() // Garante que o campo 'valores' é uma lista
      @IsNotEmpty() // Garante que a lista não está vazia
      @IsNumber({}, { each: true }) // Garante que cada item na lista é um número
      valores: number[];
    }
    
