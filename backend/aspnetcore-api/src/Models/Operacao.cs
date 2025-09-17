using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FaturamentoApi.Models
{
    [Table("operacoes", Schema = "faturamento")]
    public class Operacao
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Column("descricao")]
        public string? Descricao { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; }

        [Column("criado_em")]
        public DateTime CriadoEm { get; set; }

        // --- NOVA PROPRIEDADE ---
        [Column("meta_mensal")]
        public decimal MetaMensal { get; set; }


        // Relação: Uma operação tem muitos faturamentos
        public virtual ICollection<FaturamentoDiario> Faturamentos { get; set; } = new List<FaturamentoDiario>();
    }
}
