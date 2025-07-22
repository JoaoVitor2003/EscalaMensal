using EscalaMensal.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IItemEscalaService
    {
        Task<List<ItemEscala>> ObterPorEscalaIdAsync(int escalaId);
        Task AdicionarAsync(ItemEscala item);
        Task AtualizarAsync(ItemEscala item);
        Task RemoverAsync(int id);
    }
}
