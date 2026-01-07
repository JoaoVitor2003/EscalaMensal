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

        public ItemMissaService(IItemMissaRepository itemMissaRepository, IFuncaoRepository funcaoRepository, IUsuarioRepository usuarioRepository, IEscalaRepository escalaRepository, IMissasRepository missaRepository, IRestricaoRepository restricaoRepository)
        {
            _itemMissaRepository = itemMissaRepository;
            _funcaoRepository = funcaoRepository;
            _usuarioRepository = usuarioRepository;
            _escalaRepository = escalaRepository;
            _missaRepository = missaRepository;
            _restricaoRepository = restricaoRepository;
        }

        public async Task<List<ItemMissa>> ObterPorMissaIdAsync(int escalaId)
        {
            return await _itemMissaRepository.ObterPorMissaIdAsync(escalaId);
        }

        public async Task AdicionarAsync(ItemMissa item)
        {
            var funcao = await _funcaoRepository.ObterPorIdAsync(item.FuncaoId)
                         ?? throw new Exception("Função não encontrada.");

            Usuario? usuario = null;

            if (item.UsuarioId.HasValue)
            {
                usuario = await _usuarioRepository.ObterPorIdAsync(item.UsuarioId.Value);

                if (usuario != null)
                {
                    if (await _itemMissaRepository.ExisteUsuarioNaMissaAsync(item.MissaId, usuario.Id))
                    {
                        throw new DomainException(
                            $"O usuário '{usuario.Nome}' já está escalado para outra função nesta missa."
                        );
                    }
                }
            }

            var missa = await _missaRepository.ObterPorMissaIdAsync(item.MissaId);

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

            await _itemMissaRepository.AdicionarAsync(item);
        }

        public async Task AtualizarAsync(ItemMissa item)
        {
            var funcao = await _funcaoRepository.ObterPorIdAsync(item.FuncaoId)
                         ?? throw new Exception("Função não encontrada.");

            Usuario? usuario = null;

            if (item.UsuarioId.HasValue)
            {
                usuario = await _usuarioRepository.ObterPorIdAsync(item.UsuarioId.Value);

                if (usuario != null)
                {
                    if (await _itemMissaRepository.ExisteUsuarioNaMissaAsync(item.MissaId, usuario.Id))
                    {
                        throw new DomainException(
                            $"O usuário '{usuario.Nome}' já está escalado para outra função nesta missa."
                        );
                    }
                }
            }

            var missa = await _missaRepository.ObterPorMissaIdAsync(item.MissaId);

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

            await _itemMissaRepository.AtualizarAsync(item);
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
