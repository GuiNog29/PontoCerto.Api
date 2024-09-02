using PontoCerto.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PontoCerto.Infrastructure.Data
{
    public class PontoCertoDbContext : DbContext
    {
        public PontoCertoDbContext(DbContextOptions<PontoCertoDbContext> options) : base(options) { }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<RegistroPonto> RegistrosPontos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RegistroPonto>()
                .HasOne(x => x.Pessoa)
                .WithMany(x => x.RegistrosPonto)
                .HasForeignKey(x => x.PessoaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pessoa>()
                .HasMany(x => x.RegistrosPonto)
                .WithOne(x => x.Pessoa)
                .HasForeignKey(x => x.PessoaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pessoa>()
                .HasOne(x => x.Departamento)
                .WithMany(x => x.Pessoas)
                .HasForeignKey(x => x.DepartamentoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
