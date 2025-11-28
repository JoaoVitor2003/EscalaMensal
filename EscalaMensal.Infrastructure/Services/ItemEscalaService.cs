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
    public class ItemEscalaService : IItemEscalaService
    {
        private readonly IItemEscalaRepository _itemEscalaRepository;
        private readonly IFuncaoRepository _funcaoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ItemEscalaService(IItemEscalaRepository itemEscalaRepository, IFuncaoRepository funcaoRepository, IUsuarioRepository usuarioRepository)
        {
            _itemEscalaRepository = itemEscalaRepository;
            _funcaoRepository = funcaoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<List<ItemEscala>> ObterPorEscalaIdAsync(int escalaId)
        {
            return await _itemEscalaRepository.ObterPorEscalaIdAsync(escalaId);
        }

        public async Task AdicionarAsync(ItemEscala item)
        {
            var funcao = await _funcaoRepository.ObterPorIdAsync(item.FuncaoId);

            if (funcao == null) throw new Exception("Função não encontrada.");

            var usuario = item.UsuarioId.HasValue
                          ? await _usuarioRepository.ObterPorIdAsync(item.UsuarioId.Value)
                          : null;


            string? erro = (funcao.Id, usuario) switch
            {
                (_, null) when funcao.Obrigatoria => $"A função '{funcao.Nome}' é obrigatória.",

                (_, null) => null,


                (_, var u) when u.Cargo < funcao.Cargo => $"Usuário '{u.Nome}' não tem patente suficiente.",

                (1, var u) when u.Cargo == CargoEnum.Coroinha => "Coroinhas não podem assumir a função 1.",

                (6, var u) when u.Cargo == CargoEnum.Coroinha && u.Nivel < NivelEnum.Nivel2
                     => "Coroinhas precisam ser Nível 2 para esta função.",

                _ => null
            };

            if (erro != null)
            {
                throw new DomainException(erro);
            }

            await _itemEscalaRepository.AdicionarAsync(item);
        }

        public async Task AtualizarAsync(ItemEscala item)
        {
            await _itemEscalaRepository.AtualizarAsync(item);
        }

        public async Task RemoverAsync(int id)
        {
            await _itemEscalaRepository.RemoverAsync(id);
        }
    }
}
