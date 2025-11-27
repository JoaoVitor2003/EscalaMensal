using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
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
            var funcaoCompleta = await _funcaoRepository.ObterPorIdAsync(item.FuncaoId);
            var usuarioCompleto = item.UsuarioId.HasValue ? await _usuarioRepository.ObterPorIdAsync(item.UsuarioId.Value) : null;

            if (funcaoCompleta == null)
            {
                throw new Exception("Função não encontrada.");
            }
            var erros = new List<string>();
            if (funcaoCompleta.Obrigatoria && item.UsuarioId == null)
            {
                erros.Add($"A função '{funcaoCompleta.Nome}' é obrigatória e não foi preenchida.");
            }
            if (erros.Count > 0)
            {
                throw new Exception(string.Join(" ", erros));
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
