using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IItemEscalaRepository
    {
        Task<List<ItemEscala>> ObterPorEscalaIdAsync(int escalaId);
        Task AdicionarAsync(ItemEscala item);
        Task AtualizarAsync(ItemEscala item);
        Task RemoverAsync(int id);
    }

}
