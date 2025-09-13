using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FaturamentoApi.Models
{
    // IMPORTANTE: Mapeia a tabela 'usuarios' DENTRO do schema 'faturamento'
    [Table("usuarios", Schema = "faturamento")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("criado_em")]
        public DateTime CriadoEm { get; set; }
    }
}