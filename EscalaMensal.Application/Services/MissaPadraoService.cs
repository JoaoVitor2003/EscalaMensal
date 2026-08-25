using AutoMapper;
using EscalaMensal.Application.DTOs.MissaPadrao;
using EscalaMensal.Application.Interfaces;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Domain.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class MissaPadraoService : IMissaPadraoService
    {
        private readonly IMissaPadraoRepository _missaPadraoRepository;
        private readonly IFuncaoRepository _funcaoRepository;
        private readonly IMapper _mapper;

        public MissaPadraoService(IMissaPadraoRepository missaPadraoRepository, IFuncaoRepository funcaoRepository, IMapper mapper)
        {
            _missaPadraoRepository = missaPadraoRepository;
            _funcaoRepository = funcaoRepository;
            _mapper = mapper;
        }

        public async Task<List<MissaPadraoDto>> ObterTodasAsync()
        {
            var entities = await _missaPadraoRepository.ObterTodasAsync();
            return _mapper.Map<List<MissaPadraoDto>>(entities);
        }

        public async Task AdicionarAsync(MissaPadraoAdicionarDto dto)
        {
            var existe = await _missaPadraoRepository.ExistePorDiaHorarioAsync(dto.DiaSemana, dto.Horario);
            if (existe)
            {
                throw new DomainException("Já existe uma missa padrão cadastrada para esse dia da semana e horário.");
            }

            var entity = new MissaPadrao(dto.DiaSemana, dto.Horario, dto.CriarFuncoesPadrao);

            if (!dto.CriarFuncoesPadrao && dto.FuncoesIds != null && dto.FuncoesIds.Count > 0)
            {
                var funcoes = await _funcaoRepository.ObterPorIdsAsync(dto.FuncoesIds);
                entity.AtualizarFuncoes(funcoes);
            }

            await _missaPadraoRepository.AdicionarAsync(entity);
        }

        public async Task AtualizarAsync(int id, MissaPadraoAdicionarDto dto)
        {
            var missaPadrao = await _missaPadraoRepository.ObterPorIdAsync(id);
            if (missaPadrao == null)
            {
                throw new DomainException("Missa padrão não encontrada.");
            }

            var todas = await _missaPadraoRepository.ObterTodasAsync();
            if (todas.Any(m => m.Id != id && m.DiaSemana == dto.DiaSemana && m.Horario == dto.Horario))
            {
                throw new DomainException("Já existe uma missa padrão cadastrada para esse dia da semana e horário.");
            }

            missaPadrao.Atualizar(dto.DiaSemana, dto.Horario, dto.CriarFuncoesPadrao);

            if (!dto.CriarFuncoesPadrao && dto.FuncoesIds != null)
            {
                var funcoes = await _funcaoRepository.ObterPorIdsAsync(dto.FuncoesIds);
                missaPadrao.AtualizarFuncoes(funcoes);
            }
            else
            {
                missaPadrao.Funcoes.Clear();
            }

            await _missaPadraoRepository.AtualizarAsync(missaPadrao);
        }

        public async Task RemoverAsync(int id)
        {
            await _missaPadraoRepository.RemoverAsync(id);
        }
    }
}
