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
                .Include(h => h.HistoricoMissas)
                    .ThenInclude(m => m.HistoricoItensMissa)
                    .ThenInclude(i => i.Usuario)
                .Include(h => h.HistoricoMissas)
                    .ThenInclude(m => m.HistoricoItensMissa)
                    .ThenInclude(i => i.Funcao)
                .Where(h => h.DataInicio.Month == mes && h.DataInicio.Year == ano)
                .ToListAsync();
        }

        public async Task<List<HistoricoEscala>> ObterPorUsuarioIdAsync(int usuarioId)
        {
            return await _context.HistoricosEscala
                .Include(h => h.HistoricoMissas)
                    .ThenInclude(m => m.HistoricoItensMissa)
                    .ThenInclude(i => i.Usuario)
                .Include(h => h.HistoricoMissas)
                    .ThenInclude(m => m.HistoricoItensMissa)
                    .ThenInclude(i => i.Funcao)
                .Where(h => h.HistoricoMissas.Any(m => m.HistoricoItensMissa.Any(i => i.UsuarioId == usuarioId)))
                .OrderByDescending(h => h.DataFinalizacao)
                .ToListAsync();
        }

        public async Task AdicionarAsync(HistoricoEscala historico)
        {
            await _context.HistoricosEscala.AddAsync(historico);
            await _context.SaveChangesAsync();
        }

        public async Task<List<HistoricoEscala>> ObterTodosPorEscalaOriginalIdAsync(int escalaOriginalId)
        {
            return await _context.HistoricosEscala
                .Include(h => h.HistoricoMissas)
                    .ThenInclude(m => m.HistoricoItensMissa)
                .Where(h => h.EscalaOriginalId == escalaOriginalId)
                .ToListAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var historico = await _context.HistoricosEscala.FindAsync(id);
            if (historico != null)
            {
                _context.HistoricosEscala.Remove(historico);
                await _context.SaveChangesAsync();
            }
        }
    }
}

