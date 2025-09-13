using System.ComponentModel.DataAnnotations; // Precisamos disso para as validações

namespace FaturamentoApi.Dtos
{
    // Este é o "Contrato" que o Frontend (Vue) deve enviar no corpo (body) da requisição POST.
    public class CriarFaturamentoDto
    {
        [Required(ErrorMessage = "O ID da operação é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID da operação inválido.")]
        public int OperacaoId { get; set; }

        [Required(ErrorMessage = "A data de referência é obrigatória.")]
        public DateTime DataRef { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor do faturamento deve ser maior que zero.")]
        public decimal Valor { get; set; }
    }
}