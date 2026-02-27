using AutoMapper;
using EscalaMensal.Application.DTOs.HistoricoEscala;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class HistoricoEscalaService : IHistoricoEscalaService
    {
        private readonly IHistoricoEscalaRepository _historicoEscalaRepository;
        private readonly IMapper _mapper;

        public HistoricoEscalaService(IHistoricoEscalaRepository historicoEscalaRepository, IMapper mapper)
        {
            _historicoEscalaRepository = historicoEscalaRepository;
            _mapper = mapper;
        }

        public async Task<List<HistoricoEscalaDto>> ObterPorMesAnoAsync(int mes, int ano)
        {
            var historicos = await _historicoEscalaRepository.ObterPorMesAnoAsync(mes, ano);
            var dtos = _mapper.Map<List<HistoricoEscalaDto>>(historicos);
            return dtos;
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
