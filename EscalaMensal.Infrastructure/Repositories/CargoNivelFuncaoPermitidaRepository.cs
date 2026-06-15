using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Enums;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Context;        
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscalaMensal.Infrastructure.Repositories
{
    public class CargoNivelFuncaoPermitidaRepository : ICargoNivelFuncaoPermitidaRepository
    {
        private readonly AppDbContext _context;

        public CargoNivelFuncaoPermitidaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Funcao>> ObterFuncoesPermitidasAsync(CargoEnum cargo, NivelEnum nivel)
        {
            return await _context.CargoNivelFuncaoPermitidas
                .Where(c => c.Cargo == cargo && c.Nivel == nivel)
                .Include(c => c.Funcao)
                .Select(c => c.Funcao)
                .ToListAsync();
        }

        public async Task<List<CargoNivelFuncaoPermitida>> ObterTodasAsync()
        {
            return await _context.CargoNivelFuncaoPermitidas
                .Include(c => c.Funcao)
                .ToListAsync();
        }

        public async Task AdicionarAsync(CargoNivelFuncaoPermitida cargoNivel)
        {
            _context.CargoNivelFuncaoPermitidas.Add(cargoNivel);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var entity = await _context.CargoNivelFuncaoPermitidas.FindAsync(id);
            if (entity != null)
            {
                _context.CargoNivelFuncaoPermitidas.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
