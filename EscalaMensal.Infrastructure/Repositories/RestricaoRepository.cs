using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EscalaMensal.Infrastructure.Repositories
{
    public class RestricaoRepository : IRestricaoRepository
    {
        private readonly AppDbContext _context;

        public RestricaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Restricao>> ObterPorMesAnoAsync(int mes, int ano)
        {
            return await _context.Restricoes
                .Where(r => r.Data.Month == mes && r.Data.Year == ano)
                .Include(r => r.Usuario)
                .ToListAsync();
        }

        public async Task<List<Restricao>> ObterPorUsuarioIdAsync(int usuarioId, int mes, int ano)
        {
            return await _context.Restricoes
                .Where(r => r.UsuarioId == usuarioId && r.Data.Month == mes && r.Data.Year == ano)
                .ToListAsync();
        }

        public async Task AdicionarAsync(Restricao restricao)
        {
            await _context.Restricoes.AddAsync(restricao);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Restricao restricao)
        {
            _context.Restricoes.Update(restricao);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var restricao = await _context.Restricoes.FindAsync(id);
            if (restricao != null)
            {
                _context.Restricoes.Remove(restricao);
                await _context.SaveChangesAsync();
            }
        }
    }
}