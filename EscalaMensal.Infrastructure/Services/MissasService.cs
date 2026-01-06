using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class MissasService : IMissasService
    {
        private readonly IMissasRepository _missaRepository;
        public MissasService(IMissasRepository missaRepository)
        {
            _missaRepository = missaRepository;
        }
        public async Task AdicionarAsync(Missas missa)
        {
            var missaExistente = await _missaRepository.ExistePorDiaHorarioEscalaAsync(missa.Dia, missa.Horario, missa.EscalaId);
            if (missaExistente)
            {
                throw new Exception("Já existe uma missa cadastrada para o mesmo dia, horário nessa escala.");
            }
            await _missaRepository.AdicionarAsync(missa);
        }

        public async Task AtualizarAsync(Missas missa)
        {
            var missaExistente = await _missaRepository.ExistePorDiaHorarioEscalaAsync(missa.Dia, missa.Horario, missa.EscalaId);
            if (missaExistente)
            {
                throw new Exception("Já existe uma missa cadastrada para o mesmo dia, horário nessa escala.");
            }
            await _missaRepository.AtualizarAsync(missa);
        }

        public async Task<Missas> ObterPorMissaIdAsync(int missaId)
        {
            return await _missaRepository.ObterPorMissaIdAsync(missaId);
        }

        public async Task RemoverAsync(int id)
        {
            await _missaRepository.RemoverAsync(id);
        }
    }
}
