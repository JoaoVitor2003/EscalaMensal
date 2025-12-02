using EscalaMensal.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IItemEscalaService
    {
        Task<List<ItemMissa>> ObterPorEscalaIdAsync(int escalaId);
        Task AdicionarAsync(ItemMissa item);
        Task AtualizarAsync(ItemMissa item);
        Task RemoverAsync(int id);
    }
}
