using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class ItemEscalaService : IItemEscalaService
    {
        private readonly IItemEscalaRepository _itemEscalaRepository;

        public ItemEscalaService(IItemEscalaRepository itemEscalaRepository)
        {
            _itemEscalaRepository = itemEscalaRepository;
        }

        public async Task<List<ItemEscala>> ObterPorEscalaIdAsync(int escalaId)
        {
            return await _itemEscalaRepository.ObterPorEscalaIdAsync(escalaId);
        }

        public async Task AdicionarAsync(ItemEscala item)
        {
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
