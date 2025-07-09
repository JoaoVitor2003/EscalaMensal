using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Entities.EscalaMensal.Domain.Entities;
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

        }
    }

}
