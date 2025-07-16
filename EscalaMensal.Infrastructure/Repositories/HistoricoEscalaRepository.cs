using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EscalaMensal.Infrastructure.Repositories
{
    public class HistoricoEscalaRepository : IHistoricoEscalaRepository
    {
        private readonly AppDbContext _context;

        public HistoricoEscalaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<HistoricoEscala>> ObterPorMesAnoAsync(int mes, int ano)
        {
            return await _context.HistoricosEscala
                .Where(h => h.Data.Month == mes && h.Data.Year == ano)
                .Include(h => h.Usuario)
                .Include(h => h.Funcao)
                .ToListAsync();
        }

        public async Task<List<HistoricoEscala>> ObterPorUsuarioIdAsync(int usuarioId)
        {
            return await _context.HistoricosEscala
                .Where(h => h.UsuarioId == usuarioId)
                .Include(h => h.Funcao)
                .OrderByDescending(h => h.Data)
                .ToListAsync();
        }

        public async Task AdicionarAsync(HistoricoEscala historico)
        {
            await _context.HistoricosEscala.AddAsync(historico);
            await _context.SaveChangesAsync();
        }
    }
}
