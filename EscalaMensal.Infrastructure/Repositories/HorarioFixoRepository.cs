using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Infrastructure.Repositories
{
    public class HorarioFixoRepository : IHorarioFixoRepository
    {
        private readonly AppDbContext _context;

        public HorarioFixoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(HorarioFixo horario)
        {
            await _context.HorariosFixos.AddAsync(horario);
            await _context.SaveChangesAsync();
        }

        public async Task<HorarioFixo?> ObterPorIdAsync(int id)
        {
            return await _context.HorariosFixos.FindAsync(id);
        }

        public async Task<List<HorarioFixo>> ObterTodosAsync()
        {
            return await _context.HorariosFixos.ToListAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var horario = await _context.HorariosFixos.FindAsync(id);
            if (horario != null)
            {
                _context.HorariosFixos.Remove(horario);
                await _context.SaveChangesAsync();
            }
        }
    }
}
