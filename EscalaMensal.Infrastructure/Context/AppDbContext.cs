using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Escala> Escalas { get; set; }
        public DbSet<ItemEscala> ItensEscala { get; set; }
        public DbSet<Funcao> Funcoes { get; set; }
        public DbSet<Restricao> Restricoes { get; set; }
        public DbSet<CargoNivelFuncaoPermitida> CargoNivelFuncaoPermitidas { get; set; }
        public DbSet<HistoricoEscala> HistoricosEscala { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.UsuarioVinculado)
                .WithMany()
                .HasForeignKey(u => u.UsuarioVinculadoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ItemEscala>()
                .HasOne(i => i.Escala)
                .WithMany(e => e.Itens)
                .HasForeignKey(i => i.EscalaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemEscala>()
                .HasOne(i => i.Usuario)
                .WithMany()
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ItemEscala>()
                .HasOne(i => i.Funcao)
                .WithMany()
                .HasForeignKey(i => i.FuncaoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HistoricoEscala>()
                .HasOne(h => h.Usuario)
                .WithMany()
                .HasForeignKey(h => h.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HistoricoEscala>()
                .HasOne(h => h.Funcao)
                .WithMany()
                .HasForeignKey(h => h.FuncaoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CargoNivelFuncaoPermitida>()
                .HasIndex(c => new { c.Cargo, c.Nivel, c.FuncaoId })
                .IsUnique();

            modelBuilder.Entity<Usuario>().HasData(
    new { Id = 1, Nome = "João", Nivel = NivelEnum.Nivel3, Cargo = CargoEnum.Cerimoniario, Ativo = true },
    new { Id = 2, Nome = "Maria", Nivel = NivelEnum.Nivel1, Cargo = CargoEnum.Coroinha, Ativo = false }
);

        }

    }

}
