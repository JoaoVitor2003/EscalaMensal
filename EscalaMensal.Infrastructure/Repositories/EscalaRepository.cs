using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EscalaMensal.Infrastructure.Repositories
{
    public class EscalaRepository : IEscalaRepository
    {
        private readonly AppDbContext _context;

        public EscalaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Escala escala)
        {
            await _context.Escalas.AddAsync(escala);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Escala escala)
        {
            _context.Escalas.Update(escala);
            await _context.SaveChangesAsync();
        }

        public async Task<Escala?> ObterPorIdAsync(int id)
        {
            return await _context.Escalas
                .Include(e => e.Missas)
                    .ThenInclude(i => i.ItensMissa)
                    .ThenInclude(f => f.Funcao)
                .Include(e => e.Missas)
                    .ThenInclude(m => m.ItensMissa)
                    .ThenInclude(i => i.Usuario)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Escala?> ObterPorMesAnoAsync(int mes, int ano)
        {
            return await _context.Escalas
                .Include(e => e.Missas)
                    .ThenInclude(i => i.ItensMissa)
                .Where(e => e.Missas.Any(i => i.Dia.Month == mes && i.Dia.Year == ano))
                .FirstOrDefaultAsync();
        }

        public async Task<List<Escala>> ObterTodasAsync()
        {
            return await _context.Escalas
                .Include(e => e.Missas)
                    .ThenInclude(i => i.ItensMissa)
                .ToListAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var escala = await _context.Escalas.FindAsync(id);
            if (escala is not null)
            {
                _context.Escalas.Remove(escala);
                await _context.SaveChangesAsync();
            }
        }
    }
}
