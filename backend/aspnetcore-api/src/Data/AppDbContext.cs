using FaturamentoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FaturamentoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Operacao> Operacoes { get; set; }
        public DbSet<FaturamentoDiario> Faturamentos { get; set; }
        // A linha "public DbSet<Meta> Metas" foi removida.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("faturamento");

            modelBuilder.Entity<Operacao>(entity =>
            {
                entity.HasMany(e => e.Faturamentos)
                      .WithOne(f => f.Operacao)
                      .HasForeignKey(f => f.OperacaoId);
                
                entity.Property(e => e.MetaMensal).HasColumnType("numeric(12, 2)");
            });

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

