using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class FuncaoService : IFuncaoService
    {
        private readonly IFuncaoRepository _funcaoRepository;

        public FuncaoService(IFuncaoRepository funcaoRepository)
        {
            _funcaoRepository = funcaoRepository;
        }

        public async Task<List<Funcao>> ObterTodasAsync()
        {
            return await _funcaoRepository.ObterTodasAsync();
        }

        public async Task<Funcao?> ObterPorIdAsync(int id)
        {
            return await _funcaoRepository.ObterPorIdAsync(id);
        }

        public async Task AdicionarAsync(Funcao funcao)
        {
            await _funcaoRepository.AdicionarAsync(funcao);
        }

        public async Task AtualizarAsync(Funcao funcao)
        {
            await _funcaoRepository.AtualizarAsync(funcao);
        }

        public async Task RemoverAsync(int id)
        {
            await _funcaoRepository.RemoverAsync(id);
        }
    }
}
