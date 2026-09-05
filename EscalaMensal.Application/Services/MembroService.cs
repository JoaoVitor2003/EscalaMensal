using AutoMapper;
using EscalaMensal.Application.DTOs.Membro;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Domain.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class MembroService : IMembroService
    {
        private readonly IMembroRepository _membroRepository;
        private readonly IMapper _mapper;

        public MembroService(IMembroRepository membroRepository, IMapper mapper)
        {
            _membroRepository = membroRepository;
            _mapper = mapper;
        }

        public async Task<MembroDto?> ObterPorIdAsync(int id)
        {
            var membro = await _membroRepository.ObterPorIdAsync(id);
            var dto = _mapper.Map<MembroDto>(membro);

            return dto;
        }

        public async Task<List<MembroDto>> ObterTodosAsync()
        {
            var membros = await _membroRepository.ObterTodosAsync();
            var membrosDto = _mapper.Map<List<MembroDto>>(membros);
            return membrosDto;
        }

        public async Task AdicionarAsync(MembroAdicionarDto membro)
        {
            var membroEntity = _mapper.Map<Membro>(membro);

            await _membroRepository.AdicionarAsync(membroEntity);
        }

        public async Task AtualizarAsync(MembroAtualizarDto membroDto)
        {
            var membro = await _membroRepository.ObterPorIdAsync(membroDto.Id);

            if (membro == null)
                throw new DomainException("Membro não encontrado.");

            _mapper.Map(membroDto, membro);
            await _membroRepository.AtualizarAsync(membro);
        }

        public async Task RemoverAsync(int id)
        {
            await _membroRepository.RemoverAsync(id);
        }
    }
}
