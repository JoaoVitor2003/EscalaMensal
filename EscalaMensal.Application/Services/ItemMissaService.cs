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
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEscalaRepository _escalaRepository;
        private readonly IMissasRepository _missaRepository;
        private readonly IRestricaoRepository _restricaoRepository;
        private readonly IMapper _mapper;

        public ItemMissaService(IItemMissaRepository itemMissaRepository, IFuncaoRepository funcaoRepository, IUsuarioRepository usuarioRepository, IEscalaRepository escalaRepository, IMissasRepository missaRepository, IRestricaoRepository restricaoRepository, IMapper mapper)
        {
            _itemMissaRepository = itemMissaRepository;
            _funcaoRepository = funcaoRepository;
            _usuarioRepository = usuarioRepository;
            _escalaRepository = escalaRepository;
            _missaRepository = missaRepository;
            _restricaoRepository = restricaoRepository;
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

            Usuario? usuario = null;

            if (itemMissaEntity.UsuarioId.HasValue && itemMissaEntity.UsuarioId != 0)
            {
                usuario = await _usuarioRepository.ObterPorIdAsync(itemMissaEntity.UsuarioId.Value);

                if (usuario != null)
                {
                    if (await _itemMissaRepository.ExisteUsuarioNaMissaAsync(itemMissaEntity.MissaId, usuario.Id))
                    {
                        throw new DomainException(
                            $"O usuário '{usuario.Nome}' já está escalado para outra função nesta missa."  
                        );
                    }

                    await ValidarLimiteEscalaAsync(itemMissaEntity.MissaId, usuario.Id);
                }
            }

            var missa = await _missaRepository.ObterPorMissaIdAsync(itemMissaEntity.MissaId);

            if (usuario != null)
            {
                var restricoes = await _restricaoRepository
                    .ObterPorUsuarioIdAsync(usuario.Id, missa.Dia.Month, missa.Dia.Year);

                if (restricoes?.Any(r => DateOnly.FromDateTime(r.Data) == missa.Dia) == true)
                {
                    throw new DomainException(
                        $"O usuário '{usuario.Nome}' possui restrição para o dia {missa.Dia:dd/MM/yyyy}."    
                    );
                }
            }

            ValidarFuncaoUsuario(funcao, usuario);

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

            Usuario? usuario = null;

            if (itemMissaEntity.UsuarioId.HasValue && itemMissaEntity.UsuarioId != 0)
            {
                usuario = await _usuarioRepository.ObterPorIdAsync(itemMissaEntity.UsuarioId.Value);

                if (usuario != null)
                {
                    if (await _itemMissaRepository.ExisteUsuarioNaMissaAsync(itemMissaEntity.MissaId, usuario.Id))
                    {
                        throw new DomainException(
                            $"O usuário '{usuario.Nome}' já está escalado para outra função nesta missa."  
                        );
                    }
                    await ValidarLimiteEscalaAsync(itemMissaEntity.MissaId, usuario.Id, itemMissaEntity.Id);    
                }
            }

            var missa = await _missaRepository.ObterPorMissaIdAsync(itemMissaEntity.MissaId);

            if (usuario != null)
            {
                var restricoes = await _restricaoRepository
                    .ObterPorUsuarioIdAsync(usuario.Id, missa.Dia.Month, missa.Dia.Year);

                if (restricoes?.Any(r => DateOnly.FromDateTime(r.Data) == missa.Dia) == true)
                {
                    throw new DomainException($"O usuário '{usuario.Nome}' possui restrição para o dia {missa.Dia:dd/MM/yyyy}."
                    );
                }
            }

            ValidarFuncaoUsuario(funcao, usuario);

            itemMissaExistente.AtribuirUsuario(item.UsuarioId);

            await _itemMissaRepository.AtualizarAsync(itemMissaExistente);
        }

        private async Task ValidarLimiteEscalaAsync(int missaId, int usuarioId, int? itemMissaId = null)        
        {
            var missaInfo = await _missaRepository.ObterPorMissaIdAsync(missaId);

            var escala = await _escalaRepository.ObterPorIdAsync(missaInfo.EscalaId);

            if (escala == null)
            {
                throw new DomainException("Escala não encontrada.");
            }

            var quantidadeEscalas = await _itemMissaRepository.QuantidadeDeEscalasDoUsuarioNaEscalaAsync(missaInfo.EscalaId, usuarioId);

            if (itemMissaId.HasValue)
            {
                var itemExistente = await _itemMissaRepository.ObterPorIdAsync(itemMissaId.Value);
                if (itemExistente?.UsuarioId == usuarioId)
                {
                    if (quantidadeEscalas >= escala.LimitePermitido)
                    {
                        throw new DomainException($"Limite atingido! Esta escala permite no máximo {escala.LimitePermitido} participações por pessoa. O usuário já possui {quantidadeEscalas} missas confirmadas nesta escala mensal.");
                    }
                    return;
                }
            }
            else 
            {
                if (quantidadeEscalas >= escala.LimitePermitido)
                {
                    throw new DomainException($"Limite atingido! Esta escala permite no máximo {escala.LimitePermitido} participações por pessoa. O usuário já possui {quantidadeEscalas} missas confirmadas nesta escala mensal.");
                }
            }
        }

        private static void ValidarFuncaoUsuario(Funcao funcao, Usuario? usuario)
        {
            string? erro = (funcao.Id, usuario) switch
            {
                (_, null)
                    => null,

                (_, var u) when u.Cargo < funcao.Cargo
                    => $"Essa pessoa '{u.Nome}' não é um {funcao.Cargo} para servir nessa função.",

                (_, var u) when u.Cargo == funcao.Cargo && u.Nivel < funcao.NivelMinimo
            => $"O {u.Cargo} '{u.Nome}' precisa ser no mínimo {funcao.NivelMinimo} para esta função.",       

                _ => null
            };

            if (erro != null)
                throw new DomainException(erro);
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
