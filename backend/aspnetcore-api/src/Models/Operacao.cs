using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FaturamentoApi.Models
{
    // Mapeia esta classe para a tabela 'operacoes' no schema 'faturamento'
    [Table("operacoes", Schema = "faturamento")]
    public class Operacao
    {
        [Key] // Marca como Chave Primária
        [Column("id")] // Mapeia esta propriedade para a coluna 'id' do banco
        public int Id { get; set; }

        [Required] // Diz ao C# que este campo é obrigatório
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Column("descricao")]
        public string? Descricao { get; set; } // O '?' permite que este campo seja nulo (já que é 'TEXT' no banco)

        [Column("ativo")]
        public bool Ativo { get; set; }

        [Column("criado_em")]
        public DateTime CriadoEm { get; set; }

        // --- Propriedades de Navegação (A Mágica do EF Core) ---
        // Isso "ensina" ao C# que UMA Operacao pode ter MUITOS faturamentos.
        // O 'virtual' ajuda o EF Core a otimizar o carregamento (Lazy Loading).
        public virtual ICollection<FaturamentoDiario> Faturamentos { get; set; }

        // Isso "ensina" ao C# que UMA Operacao pode ter MUITAS metas.
        public virtual ICollection<Meta> Metas { get; set; }

        // Inicializamos as listas para evitar erros de "referência nula"
        public Operacao()
        {
            Faturamentos = new List<FaturamentoDiario>();
            Metas = new List<Meta>();
        }
    }
}