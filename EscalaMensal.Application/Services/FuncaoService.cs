using AutoMapper;
using EscalaMensal.Application.DTOs.Funcao;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class FuncaoService : IFuncaoService
    {
        private readonly IFuncaoRepository _funcaoRepository;
        private readonly IMapper _mapper;

        public FuncaoService(IFuncaoRepository funcaoRepository, IMapper mapper)
        {
            _funcaoRepository = funcaoRepository;
            _mapper = mapper;
        }

        public async Task<List<FuncaoDto>> ObterTodasAsync()
        {
            var funcoes = await _funcaoRepository.ObterTodasAsync();
            var dto = _mapper.Map<List<FuncaoDto>>(funcoes);
            return dto;
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
