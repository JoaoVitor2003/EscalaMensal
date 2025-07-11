using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EscalaMensal.Infrastructure.Repositories
{
    public class FuncaoRepository : IFuncaoRepository
    {
        private readonly AppDbContext _context;

        public FuncaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Funcao>> ObterTodasAsync()
        {
            return await _context.Funcoes.ToListAsync();
        }

        public async Task<Funcao?> ObterPorIdAsync(int id)
        {
            return await _context.Funcoes.FindAsync(id);
        }

        public async Task AdicionarAsync(Funcao funcao)
        {
            await _context.Funcoes.AddAsync(funcao);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Funcao funcao)
        {
            _context.Funcoes.Update(funcao);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var funcao = await _context.Funcoes.FindAsync(id);
            if (funcao != null)
            {
                _context.Funcoes.Remove(funcao);
                await _context.SaveChangesAsync();
            }
        }
    }
}
