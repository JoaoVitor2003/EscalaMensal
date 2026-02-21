using AutoMapper;
using EscalaMensal.Application.DTOs.Funcao;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using System;
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

        public async Task<FuncaoDto?> ObterPorIdAsync(int id)
        {
            var funcao = await _funcaoRepository.ObterPorIdAsync(id);
            var dto = _mapper.Map<FuncaoDto>(funcao);
            return dto;
        }

        public async Task AdicionarAsync(FuncaoAdicionarDto funcao)
        {
            var funcaoEntity = _mapper.Map<Funcao>(funcao);

            await _funcaoRepository.AdicionarAsync(funcaoEntity);
        }

        public async Task AtualizarAsync(FuncaoAtualizarDto funcao)
        {
            var funcaoEntity = _mapper.Map<Funcao>(funcao);
            await _funcaoRepository.AtualizarAsync(funcaoEntity);
        }

        public async Task RemoverAsync(int id)
        {
            await _funcaoRepository.RemoverAsync(id);
        }
    }
}
