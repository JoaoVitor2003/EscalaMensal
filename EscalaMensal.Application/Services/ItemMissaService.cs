using AutoMapper;
using EscalaMensal.Application.DTOs.ItemMissa;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Enums;
using EscalaMensal.Domain.Exceptions;
using EscalaMensal.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<List<ItemMissaDto>> ObterPorMissaIdAsync(int escalaId)
        {
            var itens = await _itemMissaRepository.ObterPorMissaIdAsync(escalaId);
            var dto = _mapper.Map<List<ItemMissaDto>>(itens);

            return dto;
        }

        public async Task AdicionarAsync(ItemMissaAdicionarDto item)
        {
            var itemMissaEntity = _mapper.Map<ItemMissa>(item);

            var funcao = await _funcaoRepository.ObterPorIdAsync(itemMissaEntity.FuncaoId)
                         ?? throw new Exception("Função não encontrada.");

            Usuario? usuario = null;

            if (itemMissaEntity.UsuarioId.HasValue)
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
                }
            }

            var missa = await _missaRepository.ObterPorMissaIdAsync(itemMissaEntity.MissaId);

            if (usuario != null)
            {
                var restricoes = await _restricaoRepository
                    .ObterPorUsuarioIdAsync(usuario.Id, missa.Dia.Month, missa.Dia.Year);

                if (restricoes?.Count > 0)
                {
                    throw new DomainException(
                        $"O usuário '{usuario.Nome}' possui restrição para o dia {missa.Dia:dd/MM/yyyy}."
                    );
                }
            }

            ValidarFuncaoUsuario(funcao, usuario);

            await _itemMissaRepository.AdicionarAsync(itemMissaEntity);
        }

        public async Task AtualizarAsync(ItemMissaAtualizarDto item)
        {
            var itemMissaEntity = _mapper.Map<ItemMissa>(item);
            var funcao = await _funcaoRepository.ObterPorIdAsync(itemMissaEntity.FuncaoId)
                         ?? throw new Exception("Função não encontrada.");

            Usuario? usuario = null;

            if (itemMissaEntity.UsuarioId.HasValue)
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
                }
            }

            var missa = await _missaRepository.ObterPorMissaIdAsync(itemMissaEntity.MissaId);

            if (usuario != null)
            {
                var restricoes = await _restricaoRepository
                    .ObterPorUsuarioIdAsync(usuario.Id, missa.Dia.Month, missa.Dia.Year);

                if (restricoes?.Count > 0)
                {
                    throw new DomainException(
                        $"O usuário '{usuario.Nome}' possui restrição para o dia {missa.Dia:dd/MM/yyyy}."
                    );
                }
            }

            ValidarFuncaoUsuario(funcao, usuario);

            await _itemMissaRepository.AtualizarAsync(itemMissaEntity);
        }

        private static void ValidarFuncaoUsuario(Funcao funcao, Usuario? usuario)
        {
            string? erro = (funcao.Id, usuario) switch
            {
                (_, null) when funcao.Obrigatoria
                    => $"A função '{funcao.Nome}' é obrigatória.",

                (_, null)
                    => null,

                (_, var u) when u.Cargo < funcao.Cargo
                    => $"Essa pessoa '{u.Nome}' não é um {funcao.Cargo} para servir nessa função.",

                (1, var u) when u.Cargo == CargoEnum.Coroinha
                    => "Coroinhas não podem assumir essa função.",

                (6, var u) when u.Cargo == CargoEnum.Coroinha && u.Nivel < NivelEnum.Nivel2
                    => "Coroinhas precisam ser Nível 2 para esta função.",

                _ => null
            };

            if (erro != null)
                throw new DomainException(erro);
        }



        public async Task RemoverAsync(int id)
        {
            await _itemMissaRepository.RemoverAsync(id);
        }
    }
}
