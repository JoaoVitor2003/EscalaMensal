using AutoMapper;
using EscalaMensal.Application.DTOs.HorarioFixo;
using EscalaMensal.Application.Interfaces;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class HorarioFixoService : IHorarioFixoService
    {
        private readonly IHorarioFixoRepository _repository;
        private readonly IMapper _mapper;

        public HorarioFixoService(IHorarioFixoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<HorarioFixoDto>> ObterTodosAsync()
        {
            var horarios = await _repository.ObterTodosAsync();
            return _mapper.Map<List<HorarioFixoDto>>(horarios);
        }

        public async Task AdicionarAsync(HorarioFixoAdicionarDto dto)
        {
            var horario = _mapper.Map<HorarioFixo>(dto);
            await _repository.AdicionarAsync(horario);
        }

        public async Task RemoverAsync(int id)
        {
            await _repository.RemoverAsync(id);
        }
    }
}
