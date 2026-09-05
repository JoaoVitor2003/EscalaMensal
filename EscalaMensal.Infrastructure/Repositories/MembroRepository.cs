using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EscalaMensal.Infrastructure.Repositories
{
    public class MembroRepository : IMembroRepository
    {
        private readonly AppDbContext _context;

        public MembroRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Membro membro)
        {
            await _context.Membros.AddAsync(membro);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Membro membro)
        {
            _context.Membros.Update(membro);
            await _context.SaveChangesAsync();
        }

        public async Task<Membro?> ObterPorIdAsync(int id)
        {
            return await _context.Membros
                .Include(u => u.MembroVinculado)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<Membro>> ObterTodosAsync()
        {
            return await _context.Membros.ToListAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var membro = await _context.Membros.FindAsync(id);
            if (membro != null)
            {
                _context.Membros.Remove(membro);
                await _context.SaveChangesAsync();
            }
        }
    }
}
