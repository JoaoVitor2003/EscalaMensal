using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Infrastructure.Repositories
{
    public class MissaPadraoRepository : IMissaPadraoRepository
    {
        private readonly AppDbContext _context;

        public MissaPadraoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MissaPadrao>> ObterTodasAsync()
        {
            return await _context.MissasPadrao.ToListAsync();
        }

        public async Task AdicionarAsync(MissaPadrao missaPadrao)
        {
            await _context.MissasPadrao.AddAsync(missaPadrao);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var entity = await _context.MissasPadrao.FindAsync(id);
            if (entity != null)
            {
                _context.MissasPadrao.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<MissaPadrao?> ObterPorIdAsync(int id)
        {
            return await _context.MissasPadrao.FindAsync(id);
        }

        public async Task<bool> ExistePorDiaHorarioAsync(DayOfWeek diaSemana, TimeOnly horario)
        {
            return await _context.MissasPadrao
                .AnyAsync(m => m.DiaSemana == diaSemana && m.Horario == horario);
        }
    }
}
