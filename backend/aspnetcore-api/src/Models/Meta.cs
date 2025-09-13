using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FaturamentoApi.Models
{
    [Table("metas", Schema = "faturamento")]
    public class Meta
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("mes")]
        public int Mes { get; set; }

        [Column("ano")]
        public int Ano { get; set; }

        [Column("valor_meta")]
        public decimal ValorMeta { get; set; } // Note que o C# usa PascalCase (ValorMeta) para mapear o snake_case (valor_meta) do SQL

        [Column("criado_em")]
        public DateTime CriadoEm { get; set; }

        // --- Chave Estrangeira (A Ligação) ---
        [Column("operacao_id")]
        public int OperacaoId { get; set; }

        // --- Propriedade de Navegação ---
        // Assim como o Faturamento, a Meta também pertence a UMA Operação.
        [ForeignKey("OperacaoId")]
        public virtual Operacao? Operacao { get; set; }
    }
}