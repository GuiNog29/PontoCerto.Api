using PontoCerto.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PontoCerto.Infrastructure.Data
{
    public class PontoCertoDbContext : DbContext
    {
        public PontoCertoDbContext(DbContextOptions<PontoCertoDbContext> options) : base(options) { }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<RegistroPonto> RegistrosPontos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RegistroPonto>()
                .HasOne(t => t.Pessoa)
                .WithMany(u => u.RegistrosPontos)
                .HasForeignKey(t => t.PessoaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pessoa>()
                .HasMany(x => x.RegistrosPontos)
                .WithOne(x => x.Pessoa)
                .HasForeignKey(x => x.PessoaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
