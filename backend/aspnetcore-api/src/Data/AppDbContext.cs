using FaturamentoApi.Models; // Importa TODOS os nossos models
using Microsoft.EntityFrameworkCore;

namespace FaturamentoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // --- REGISTRO DAS TABELAS (DbSets) ---
        // Dizemos ao EF Core: "Você é responsável pela tabela 'Usuarios'"
        public DbSet<Usuario> Usuarios { get; set; }

        // NOVOS REGISTROS:
        public DbSet<Operacao> Operacoes { get; set; }
        public DbSet<FaturamentoDiario> Faturamentos { get; set; }
        public DbSet<Meta> Metas { get; set; }


        // --- CONFIGURAÇÃO FINA (Fluent API) ---
        // Aqui configuramos detalhes que os [Atributos] nos models não conseguem
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Diz ao EF para usar o schema 'faturamento' como padrão para todas as tabelas
            modelBuilder.HasDefaultSchema("faturamento");
            base.OnModelCreating(modelBuilder);

            // --- Configurando a entidade Operacao ---
            modelBuilder.Entity<Operacao>(entity =>
            {
                // Configura a relação (Embora o EF já entenda isso pelas nossas propriedades de navegação, é bom ser explícito):

                // "UMA Operacao TEM MUITOS Faturamentos"
                entity.HasMany(operacao => operacao.Faturamentos)
                      .WithOne(faturamento => faturamento.Operacao) // "Um Faturamento TEM UMA Operacao"
                      .HasForeignKey(faturamento => faturamento.OperacaoId); // "A ligação é pela chave OperacaoId"

                // "UMA Operacao TEM MUITAS Metas"
                entity.HasMany(operacao => operacao.Metas)
                      .WithOne(meta => meta.Operacao)
                      .HasForeignKey(meta => meta.OperacaoId);
            });

            // --- Configurando a entidade FaturamentoDiario ---
            modelBuilder.Entity<FaturamentoDiario>(entity =>
            {
                // Garante que o EF use o tipo de coluna 'date' (e não timestamp)
                entity.Property(f => f.Data).HasColumnType("date");

                // Garante que o EF use o tipo exato do banco (precisão 12, 2 casas decimais)
                entity.Property(f => f.Valor).HasColumnType("numeric(12, 2)");

                // IMPORTANTE: Recria a regra UNIQUE que fizemos no SQL.
                // Isso impede que alguém cadastre dois faturamentos para a MESMA operação no MESMO dia.
                entity.HasIndex(f => new { f.OperacaoId, f.Data }).IsUnique();

                // FILTRO GLOBAL (A MÁGICA):
                // Isto adiciona "WHERE ativo = TRUE" em TODAS as consultas SELECT
                // feitas para a tabela Faturamentos.
                entity.HasQueryFilter(f => f.Ativo);
            });

            // --- Configurando a entidade Meta ---
            modelBuilder.Entity<Meta>(entity =>
            {
                // Garante o tipo exato para o valor da meta.
                entity.Property(m => m.ValorMeta).HasColumnType("numeric(12, 2)");
            });
        }
    }
}