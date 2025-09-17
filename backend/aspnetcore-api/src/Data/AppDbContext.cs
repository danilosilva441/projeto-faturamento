using FaturamentoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FaturamentoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        // Mapeamento das tabelas para DbSets
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Operacao> Operacoes { get; set; }
        public DbSet<FaturamentoDiario> Faturamentos { get; set; }
        // public DbSet<Meta> Metas { get; set; } // <-- REMOVEMOS ESTA LINHA

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("faturamento");

            // Entidade Operacao
            modelBuilder.Entity<Operacao>(entity =>
            {
                entity.HasMany(e => e.Faturamentos)
                      .WithOne(f => f.Operacao)
                      .HasForeignKey(f => f.OperacaoId);
                
                // Garante que o tipo de coluna 'meta_mensal' seja numeric(12, 2)
                entity.Property(e => e.MetaMensal).HasColumnType("numeric(12, 2)");
            });

            // Entidade FaturamentoDiario
            modelBuilder.Entity<FaturamentoDiario>(entity =>
            {
                entity.Property(e => e.Valor).HasColumnType("numeric(12, 2)");
                entity.Property(e => e.Data).HasColumnType("date");
                entity.HasIndex(e => new { e.OperacaoId, e.Data }).IsUnique();
                entity.HasQueryFilter(f => f.Ativo); 
            });
        }
    }
}
