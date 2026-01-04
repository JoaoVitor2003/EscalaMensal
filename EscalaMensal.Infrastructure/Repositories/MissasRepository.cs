using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Infrastructure.Repositories
{
    public class MissasRepository : IMissasRepository
    {
        private readonly AppDbContext _context;
        public MissasRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AdicionarAsync(Missas missa)
        {
            await _context.Missas.AddAsync(missa);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Missas missa)
        {
            _context.Missas.Update(missa);
            await _context.SaveChangesAsync();
        }

        public Task<bool> ExistePorDiaHorarioEscalaAsync(DateOnly dia, TimeOnly horario, int escalaId)
        {
            return _context.Missas
                .AnyAsync(m => m.Dia == dia && m.Horario == horario && m.EscalaId == escalaId);
        }

        public async Task<Missas> ObterPorMissaIdAsync(int missaId)
        {
            return await _context.Missas
                    .Include(m => m.ItensMissa)
                    .ThenInclude(f => f.Funcao)
                    .Include(m => m.ItensMissa)
                    .ThenInclude(u => u.Usuario)
                    .FirstAsync(m => m.Id == missaId);
        }

        public async Task RemoverAsync(int id)
        {
            var missas = await _context.Missas.FindAsync(id);
            if(missas != null)
            {
                _context.Missas.Remove(missas);
                await _context.SaveChangesAsync();
            }
        }
    }
}
