using System.ComponentModel.DataAnnotations;

namespace FaturamentoApi.Dtos
{
    // Este é o "Contrato" para o PUT. Note que ele NÃO TEM o OperacaoId,
    // pois não vamos permitir que o lançamento mude de "dono".
    public class AtualizarFaturamentoDto
    {
        [Required(ErrorMessage = "A data de referência é obrigatória.")]
        public DateTime DataRef { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor do faturamento deve ser maior que zero.")]
        public decimal Valor { get; set; }
    }
}