using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Infrastructure.Services
{
    public class MissaService : IMissaService
    {
        private readonly MissasRepository _missaRepository;
        public MissaService(MissasRepository missaRepository)
        {
            _missaRepository = missaRepository;
        }
        public async Task AdicionarAsync(Missas missa)
        {
            await _missaRepository.AdicionarAsync(missa);
        }

        public async Task AtualizarAsync(Missas missa)
        {
            await _missaRepository.AtualizarAsync(missa);
        }

        public async Task<List<Missas>> ObterPorEscalaIdAsync(int escalaId)
        {
            return await _missaRepository.ObterPorEscalaIdAsync(escalaId);
        }

        public async Task RemoverAsync(int id)
        {
            await _missaRepository.RemoverAsync(id);
        }
    }
}
