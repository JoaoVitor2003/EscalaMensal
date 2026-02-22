using AutoMapper;
using EscalaMensal.Application.DTOs.Restricao;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class RestricaoService : IRestricaoService
    {
        private readonly IRestricaoRepository _restricaoRepository;
        private readonly IMapper _mapper;

        public RestricaoService(IRestricaoRepository restricaoRepository, IMapper mapper)
        {
            _restricaoRepository = restricaoRepository;
            _mapper = mapper;
        }

        public async Task<List<RestricaoDto>> ObterPorMesAnoAsync(int mes, int ano)
        {
            var restricoes = await _restricaoRepository.ObterPorMesAnoAsync(mes, ano);
            var restricoesDto = _mapper.Map<List<RestricaoDto>>(restricoes);
            return restricoesDto;
        }

        public async Task<List<RestricaoDto>> ObterPorUsuarioIdAsync(int usuarioId, int mes, int ano)
        {
            var restricoes = await _restricaoRepository.ObterPorUsuarioIdAsync(usuarioId, mes, ano);
            var restricoesDto = _mapper.Map<List<RestricaoDto>>(restricoes);
            return restricoesDto;
        }

        public async Task AdicionarAsync(RestricaoAdicionarDto restricao)
        {
            var restricaoEntity = _mapper.Map<Restricao>(restricao);
            await _restricaoRepository.AdicionarAsync(restricaoEntity);
        }

        public async Task AtualizarAsync(RestricaoAtualizarDto restricao)
        {
            var restricaoEntity = _mapper.Map<Restricao>(restricao);
            await _restricaoRepository.AtualizarAsync(restricaoEntity);
        }

        public async Task RemoverAsync(int id)
        {
            await _restricaoRepository.RemoverAsync(id);
        }
    }
}
