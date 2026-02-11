using EscalaMensal.Application.DTOs.ItemMissa;
using EscalaMensal.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IItemMissaService
    {
        Task<List<ItemMissaDto>> ObterPorMissaIdAsync(int escalaId);
        Task AdicionarAsync(ItemMissaAdicionarDto item);
        Task AtualizarAsync(ItemMissaAtualizarDto item);
        Task RemoverAsync(int id);
    }
}
