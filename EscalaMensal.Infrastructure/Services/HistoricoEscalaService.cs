using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class HistoricoEscalaService : IHistoricoEscalaService
    {
        private readonly IHistoricoEscalaRepository _historicoEscalaRepository;

        public HistoricoEscalaService(IHistoricoEscalaRepository historicoEscalaRepository)
        {
            _historicoEscalaRepository = historicoEscalaRepository;
        }

        public async Task<List<HistoricoEscala>> ObterPorMesAnoAsync(int mes, int ano)
        {
            return await _historicoEscalaRepository.ObterPorMesAnoAsync(mes, ano);
        }

        public async Task<List<HistoricoEscala>> ObterPorUsuarioIdAsync(int usuarioId)
        {
            return await _historicoEscalaRepository.ObterPorUsuarioIdAsync(usuarioId);
        }

        public async Task AdicionarAsync(HistoricoEscala historico)
        {
            await _historicoEscalaRepository.AdicionarAsync(historico);
        }
    }
}
