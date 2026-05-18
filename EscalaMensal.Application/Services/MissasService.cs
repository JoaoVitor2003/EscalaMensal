using AutoMapper;
using EscalaMensal.Application.DTOs.Missa;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;

namespace EscalaMensal.Application.Services
{
    public class MissasService : IMissasService
    {
        private readonly IMissasRepository _missaRepository;
        private readonly IFuncaoRepository _funcaoRepository;
        private readonly IMapper _mapper;
        public MissasService(IMissasRepository missaRepository, IFuncaoRepository funcaoRepository, IMapper mapper)
        {
            _missaRepository = missaRepository;
            _funcaoRepository = funcaoRepository;
            _mapper = mapper;
        }
        public async Task<MissasDto> AdicionarAsync(MissaAdicionarDto missa)
        {
            var missaEntity = _mapper.Map<Missas>(missa);
            var missaExistente = await _missaRepository.ExistePorDiaHorarioEscalaAsync(missa.Dia, missa.Horario, missa.EscalaId);
            if (missaExistente)
            {
                throw new Exception("Já existe uma missa cadastrada para o mesmo dia, horário nessa escala.");
            }

            var funcoesObrigatorias = await _funcaoRepository.ObterObrigatoriasAsync();
            foreach (var funcao in funcoesObrigatorias)
            {
                missaEntity.ItensMissa.Add(new ItemMissa(missaId: 0, funcaoId: funcao.Id));
            }

            await _missaRepository.AdicionarAsync(missaEntity);
            return _mapper.Map<MissasDto>(missaEntity);
        }

        public async Task AtualizarAsync(MissaAtualizarDto missa)
        {
            var missaExistente = await _missaRepository.ExistePorDiaHorarioEscalaAsync(missa.Dia, missa.Horario, missa.EscalaId);
            if (missaExistente)
            {
                throw new Exception("Já existe uma missa cadastrada para o mesmo dia, horário nessa escala.");
            }
            var missaEntity = _mapper.Map<Missas>(missa);
            await _missaRepository.AtualizarAsync(missaEntity);
        }

        public async Task<MissasDto> ObterPorMissaIdAsync(int missaId)
        {
            var missa = await _missaRepository.ObterPorMissaIdAsync(missaId);
            var dto = _mapper.Map<MissasDto>(missa);
            return dto;
        }

        public async Task RemoverAsync(int id)
        {
            await _missaRepository.RemoverAsync(id);
        }
    }
}
