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
                    .ThenInclude(i => i.Membro)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Escala?> ObterPorMesAnoAsync(int mes, int ano, int? usuarioId = null, int? perfil = null, int? paroquiaId = null)
        {
            var query = _context.Escalas
                .Include(e => e.Missas)
                    .ThenInclude(i => i.ItensMissa)
                .Where(e => e.Missas.Any(i => i.Dia.Month == mes && i.Dia.Year == ano));

            if (paroquiaId.HasValue)
            {
                query = query.Where(e => e.ParoquiaId == paroquiaId.Value);
            }

            if (usuarioId.HasValue && perfil != 1)
            {
                query = query.Where(e => e.CriadoPorUsuarioId == usuarioId.Value);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Escala>> ObterTodasAsync(int? usuarioId = null, int? perfil = null, int? paroquiaId = null)
        {
            var query = _context.Escalas
                .Include(e => e.Missas)
                    .ThenInclude(i => i.ItensMissa)
                    .ThenInclude(i => i.Funcao)
                .Include(e => e.Missas)
                    .ThenInclude(i => i.ItensMissa)
                    .ThenInclude(i => i.Membro)
                .AsQueryable();

            if (paroquiaId.HasValue)
            {
                query = query.Where(e => e.ParoquiaId == paroquiaId.Value);
            }

            if (usuarioId.HasValue && perfil != 1)
            {
                query = query.Where(e => e.CriadoPorUsuarioId == usuarioId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var escala = await _context.Escalas
                    .Include(e => e.Missas)
                        .ThenInclude(m => m.ItensMissa)
                    .FirstOrDefaultAsync(e => e.Id == id);

            if (escala is not null)
            {
                _context.Escalas.Remove(escala);
                await _context.SaveChangesAsync();
            }
        }
    }
}
