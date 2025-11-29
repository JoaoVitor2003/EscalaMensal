using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class EscalaService : IEscalaService
    {
        private readonly IEscalaRepository _escalaRepository;

        public EscalaService(IEscalaRepository escalaRepository)
        {
            _escalaRepository = escalaRepository;
        }

        public async Task<Escala?> ObterPorMesAnoAsync(int mes, int ano)
        {
            return await _escalaRepository.ObterPorMesAnoAsync(mes, ano);
        }

        public async Task<List<Escala>> ObterTodasAsync()
        {
            return await _escalaRepository.ObterTodasAsync();
        }

        public async Task AdicionarAsync(Escala escala)
        {
            await _escalaRepository.AdicionarAsync(escala);
        }

        public async Task AtualizarAsync(Escala escala)
        {
            await _escalaRepository.AtualizarAsync(escala);
        }

        public async Task RemoverAsync(int id)
        {
            await _escalaRepository.RemoverAsync(id);
        }

        public async Task<Escala?> ObterPorIdAsync(int id)
        {
            return await _escalaRepository.ObterPorIdAsync(id);
        }
    }
}
