using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FaturamentoApi.Models
{
    [Table("faturamentos", Schema = "faturamento")]
    public class FaturamentoDiario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("data")]
        public DateTime Data { get; set; } // O EF Core vai converter isso para o tipo 'DATE' do Postgres

        [Column("valor")]
        public decimal Valor { get; set; } // O 'decimal' do C# mapeia perfeitamente o 'NUMERIC' do Postgres

        [Column("criado_em")]
        public DateTime CriadoEm { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; }

        public FaturamentoDiario()
        {
            Ativo = true; // Garante que todo novo lançamento já comece como Ativo
        }

        // --- Chave Estrangeira (A Ligação) ---
        [Column("operacao_id")]
        public int OperacaoId { get; set; } // Esta é a coluna que REALMENTE guarda o ID da operação

        // --- Propriedade de Navegação (A Mágica do EF Core) ---
        // Isso "ensina" ao C# que este FaturamentoDiario pertence a UMA Operação.
        // O EF usará o [ForeignKey] para saber que a propriedade 'OperacaoId' acima é a chave para este objeto.
        [ForeignKey("OperacaoId")]
        public virtual Operacao? Operacao { get; set; }
    }
}