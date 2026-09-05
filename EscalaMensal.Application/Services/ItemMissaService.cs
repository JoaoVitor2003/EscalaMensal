using System.Linq;
using AutoMapper;
using EscalaMensal.Application.DTOs.ItemMissa;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Enums;
using EscalaMensal.Domain.Exceptions;
using EscalaMensal.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class ItemMissaService : IItemMissaService
    {
        private readonly IItemMissaRepository _itemMissaRepository;
        private readonly IFuncaoRepository _funcaoRepository;
        private readonly IMembroRepository _membroRepository;
        private readonly IEscalaRepository _escalaRepository;
        private readonly IMissasRepository _missaRepository;
        private readonly IRestricaoRepository _restricaoRepository;
        private readonly ICargoNivelFuncaoPermitidaRepository _cargoNivelFuncaoPermitidaRepository;
        private readonly IMapper _mapper;

        public ItemMissaService(IItemMissaRepository itemMissaRepository, IFuncaoRepository funcaoRepository, IMembroRepository membroRepository, IEscalaRepository escalaRepository, IMissasRepository missaRepository, IRestricaoRepository restricaoRepository, ICargoNivelFuncaoPermitidaRepository cargoNivelFuncaoPermitidaRepository, IMapper mapper)
        {
            _itemMissaRepository = itemMissaRepository;
            _funcaoRepository = funcaoRepository;
            _membroRepository = membroRepository;
            _escalaRepository = escalaRepository;
            _missaRepository = missaRepository;
            _restricaoRepository = restricaoRepository;
            _cargoNivelFuncaoPermitidaRepository = cargoNivelFuncaoPermitidaRepository;
            _mapper = mapper;
        }

        public async Task<ItemMissaDto> ObterPorMissaIdAsync(int escalaId)
        {
            var itens = await _itemMissaRepository.ObterPorMissaIdAsync(escalaId);
            var dto = _mapper.Map<ItemMissaDto>(itens);

            return dto;
        }

        public async Task AdicionarAsync(ItemMissaAdicionarDto item)
        {
            var itemMissaEntity = _mapper.Map<ItemMissa>(item);

            var funcao = await _funcaoRepository.ObterPorIdAsync(itemMissaEntity.FuncaoId)
                         ?? throw new DomainException("Função não encontrada.");

            var jaExisteEssaFuncaoNaMissa = await _itemMissaRepository.ExisteFuncaoNaMissaAsync(itemMissaEntity.MissaId, funcao.Id);

            if (jaExisteEssaFuncaoNaMissa && !funcao.EhMultipla)
            {
                throw new DomainException($"A função '{funcao.Nome}' não permite mais de uma escala nesta missa.");
            }

            Membro? membro = null;

            if (itemMissaEntity.MembroId.HasValue && itemMissaEntity.MembroId != 0)
            {
                membro = await _membroRepository.ObterPorIdAsync(itemMissaEntity.MembroId.Value);

                if (membro != null)
                {
                    if (await _itemMissaRepository.ExisteMembroNaMissaAsync(itemMissaEntity.MissaId, membro.Id))
                    {
                        throw new DomainException(
                            $"O membro '{membro.Nome}' já está escalado para outra função nesta missa."
                        );
                    }

                    await ValidarLimiteEscalaAsync(itemMissaEntity.MissaId, membro.Id);
                }
            }

            var missa = await _missaRepository.ObterPorMissaIdAsync(itemMissaEntity.MissaId);

            if (membro != null)
            {
                await ValidarRestricaoEDisponibilidadeDiaEHorarioAsync(membro, missa);
            }

            await ValidarFuncaoMembroAsync(funcao, membro);

            await _itemMissaRepository.AdicionarAsync(itemMissaEntity);
        }

        public async Task UpdateAsync(ItemMissaAtualizarDto item)
        {
            var itemMissaExistente = await _itemMissaRepository.ObterPorIdAsync(item.Id);
            if (itemMissaExistente == null)
            {
                throw new DomainException($"Item da missa com ID {item.Id} não encontrado.");
            }
            ItemMissa itemMissaEntity = _mapper.Map(item, itemMissaExistente);
            var funcao = await _funcaoRepository.ObterPorIdAsync(itemMissaEntity.FuncaoId)
                         ?? throw new DomainException("Função não encontrada.");

            Membro? membro = null;

            if (itemMissaEntity.MembroId.HasValue && itemMissaEntity.MembroId != 0)
            {
                membro = await _membroRepository.ObterPorIdAsync(itemMissaEntity.MembroId.Value);

                if (membro != null)
                {
                    if (await _itemMissaRepository.ExisteMembroNaMissaAsync(itemMissaEntity.MissaId, membro.Id))
                    {
                        throw new DomainException(
                            $"O membro '{membro.Nome}' já está escalado para outra função nesta missa."
                        );
                    }
                    await ValidarLimiteEscalaAsync(itemMissaEntity.MissaId, membro.Id, itemMissaEntity.Id);
                }
            }

            var missa = await _missaRepository.ObterPorMissaIdAsync(itemMissaEntity.MissaId);

            if (membro != null)
            {
                await ValidarRestricaoEDisponibilidadeDiaEHorarioAsync(membro, missa);
            }

            await ValidarFuncaoMembroAsync(funcao, membro);

            itemMissaExistente.AtribuirMembro(item.MembroId);

            await _itemMissaRepository.AtualizarAsync(itemMissaExistente);
        }

        private async Task ValidarRestricaoEDisponibilidadeDiaEHorarioAsync(Membro membro, Missas missa)
        {
            var restricoes = await _restricaoRepository
                .ObterPorMembroIdAsync(membro.Id, missa.Dia.Month, missa.Dia.Year);

            if (restricoes?.Any(r => DateOnly.FromDateTime(r.Data) == missa.Dia) == true)
            {
                throw new DomainException(
                    $"O membro '{membro.Nome}' possui restrição para o dia {missa.Dia:dd/MM/yyyy}."
                );
            }

            if (!membro.DiasDisponiveis.Contains(missa.Dia.DayOfWeek))
            {
                string diaSemanaPt = missa.Dia.DayOfWeek switch
                {
                    DayOfWeek.Sunday => "domingo",
                    DayOfWeek.Monday => "segunda-feira",
                    DayOfWeek.Tuesday => "terça-feira",
                    DayOfWeek.Wednesday => "quarta-feira",
                    DayOfWeek.Thursday => "quinta-feira",
                    DayOfWeek.Friday => "sexta-feira",
                    DayOfWeek.Saturday => "sábado",
                    _ => missa.Dia.DayOfWeek.ToString()
                };

                throw new DomainException(
                    $"O membro '{membro.Nome}' não está disponível para servir de {diaSemanaPt}."
                );
            }

            if (!membro.HorasPreferenciais.Any(h => h.Hour == missa.Horario.Hour && h.Minute == missa.Horario.Minute))
            {
                throw new DomainException(
                    $"O membro '{membro.Nome}' não está disponível para servir no horário das {missa.Horario:HH:mm}."
                );
            }
        }

        private async Task ValidarLimiteEscalaAsync(int missaId, int membroId, int? itemMissaId = null)
        {
            var missaInfo = await _missaRepository.ObterPorMissaIdAsync(missaId);

            var escala = await _escalaRepository.ObterPorIdAsync(missaInfo.EscalaId);

            if (escala == null)
            {
                throw new DomainException("Escala não encontrada.");
            }

            var quantidadeEscalas = await _itemMissaRepository.QuantidadeDeEscalasDoMembroNaEscalaAsync(missaInfo.EscalaId, membroId);

            if (itemMissaId.HasValue)
            {
                var itemExistente = await _itemMissaRepository.ObterPorIdAsync(itemMissaId.Value);
                if (itemExistente?.MembroId == membroId)
                {
                    if (quantidadeEscalas >= escala.LimitePermitido)
                    {
                        throw new DomainException($"Limite atingido! Esta escala permite no máximo {escala.LimitePermitido} participações por pessoa. O membro já possui {quantidadeEscalas} missas confirmadas nesta escala mensal.");
                    }
                    return;
                }
            }
            else 
            {
                if (quantidadeEscalas >= escala.LimitePermitido)
                {
                    throw new DomainException($"Limite atingido! Esta escala permite no máximo {escala.LimitePermitido} participações por pessoa. O membro já possui {quantidadeEscalas} missas confirmadas nesta escala mensal.");
                }
            }
        }

        private async Task ValidarFuncaoMembroAsync(Funcao funcao, Membro? membro)
        {
            if (membro == null)
            {
                return;
            }

            if (membro.Cargo == CargoEnum.Coroinha && funcao.Cargo == CargoEnum.Cerimoniario)
            {
                throw new DomainException($"Essa pessoa '{membro.Nome}' não é um {funcao.Cargo} para servir nessa função.");
            }

            var todasPermissoes = await _cargoNivelFuncaoPermitidaRepository.ObterTodasAsync();

            var temPermissao = todasPermissoes.Any(p => 
                p.FuncaoId == funcao.Id &&
                ((p.Cargo == membro.Cargo && p.Nivel <= membro.Nivel) || (p.Cargo < membro.Cargo))
            );

            if (!temPermissao)
            {
                throw new DomainException(
                    $"'{membro.Nome}' não tem permissão para a função '{funcao.Nome}' com base no seu cargo ({membro.Cargo}) e nível ({membro.Nivel.ToString().Replace("Nivel", "Nível")})."
                );
            }
        }

        public async Task RemoverAsync(int id)
        {
            await _itemMissaRepository.RemoverAsync(id);
        }

        public async Task AtualizarOrdemAsync(List<AtualizarOrdemItemMissaDto> itens)
        {
            var entidades = itens
                .Select(x => new ItemMissa(
                    x.Id,
                    x.Ordem))
                .ToList();

            await _itemMissaRepository.AtualizarOrdemItensMissa(entidades);
        }

        public async Task AtualizarAsync(ItemMissaAtualizarDto item)
        {
            await UpdateAsync(item);
        }
    }
}
