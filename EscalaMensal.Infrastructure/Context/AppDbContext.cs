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
        public DbSet<Missas> Missas { get; set; }
        public DbSet<ItemMissa> ItensMissa { get; set; }
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

            modelBuilder.Entity<ItemMissa>()
                .HasOne(i => i.Missas)
                .WithMany(e => e.Itens)
                .HasForeignKey(i => i.MissaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemMissa>()
                .HasOne(i => i.Usuario)
                .WithMany()
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ItemMissa>()
                .HasOne(i => i.Missas)
                .WithMany()
                .HasForeignKey(i => i.MissaId)
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
    new { Id = 1, Nome = "João", Nivel = NivelEnum.Nivel3, Cargo = CargoEnum.Cerimoniario, Ativo = true, HoraPreferencial = TimeOnly.Parse("10:00"), DisponivelQuarta = false, DisponivelSabado = true, DisponivelQuinta = false },
    new { Id = 2, Nome = "Maria", Nivel = NivelEnum.Nivel1, Cargo = CargoEnum.Coroinha, Ativo = false, HoraPreferencial = TimeOnly.Parse("07:30"), DisponivelQuarta = true, DisponivelSabado = true, DisponivelQuinta = true }
);

            modelBuilder.Entity<Funcao>().HasData(
                new Funcao(1, "Cerimoniário Geral", true)
                {
                    Abreviacao = "CG",
                    Cargo = CargoEnum.Cerimoniario,
                    NivelMinimo = NivelEnum.Nivel3
                },
                new Funcao(2,"Cerimoniário Auxiliar 1", false)
                {
                    Abreviacao = "CA1",
                    Cargo = CargoEnum.Cerimoniario,
                    NivelMinimo = NivelEnum.Nivel1
                },
                new Funcao(3, "Cerimoniário Auxiliar 2", false)
                {
                    Abreviacao = "CA2",
                    Cargo = CargoEnum.Cerimoniario,
                    NivelMinimo = NivelEnum.Nivel1
                },
                new Funcao(4, "Librífero", true)
                {
                    Abreviacao = "Li",
                    Cargo = CargoEnum.Cerimoniario,
                    NivelMinimo = NivelEnum.Nivel3
                },
                new Funcao(5, "Leitor", false)
                {
                    Abreviacao = "L",
                    Cargo = CargoEnum.Coroinha,
                    NivelMinimo = NivelEnum.Nivel2
                },
                new Funcao(6, "Ceroferario 1", false)
                {
                    Abreviacao = "Ce1",
                    Cargo = CargoEnum.Coroinha,
                    NivelMinimo = NivelEnum.Nivel3
                },
                new Funcao(7, "Ceroferario 2", false)
                {
                    Abreviacao = "Ce2",
                    Cargo = CargoEnum.Coroinha,
                    NivelMinimo = NivelEnum.Nivel3
                },
                new Funcao(8, "Cruciferário", false)
                {
                    Abreviacao = "Cr",
                    Cargo = CargoEnum.Coroinha,
                    NivelMinimo = NivelEnum.Nivel2
                },
                new Funcao(9, "Ofertório", false)
                {
                    Abreviacao = "O",
                    Cargo = CargoEnum.Coroinha,
                    NivelMinimo = NivelEnum.Nivel1
                },
                new Funcao(10, "Turibulo", false)
                {
                    Abreviacao = "T",
                    Cargo = CargoEnum.Cerimoniario,
                    NivelMinimo = NivelEnum.Nivel2
                },
                new Funcao(11, "Naveta", false)
                {
                    Abreviacao = "N",
                    Cargo = CargoEnum.Coroinha,
                    NivelMinimo = NivelEnum.Nivel1
                },
                new Funcao(12, "Cerimoniário de Procissão", false)
                {
                    Abreviacao = "CP",
                    Cargo = CargoEnum.Cerimoniario,
                    NivelMinimo = NivelEnum.Nivel1
                }
            );

        }

    }

}
