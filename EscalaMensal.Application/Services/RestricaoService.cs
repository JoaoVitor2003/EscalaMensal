using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class RestricaoService : IRestricaoService
    {
        private readonly IRestricaoRepository _restricaoRepository;

        public RestricaoService(IRestricaoRepository restricaoRepository)
        {
            _restricaoRepository = restricaoRepository;
        }

        public async Task<List<Restricao>> ObterPorMesAnoAsync(int mes, int ano)
        {
            return await _restricaoRepository.ObterPorMesAnoAsync(mes, ano);
        }

        public async Task<List<Restricao>> ObterPorUsuarioIdAsync(int usuarioId, int mes, int ano)
        {
            return await _restricaoRepository.ObterPorUsuarioIdAsync(usuarioId, mes, ano);
        }

        public async Task AdicionarAsync(Restricao restricao)
        {
            await _restricaoRepository.AdicionarAsync(restricao);
        }

        public async Task AtualizarAsync(Restricao restricao)
        {
            await _restricaoRepository.AtualizarAsync(restricao);
        }

        public async Task RemoverAsync(int id)
        {
            await _restricaoRepository.RemoverAsync(id);
        }
    }
}
