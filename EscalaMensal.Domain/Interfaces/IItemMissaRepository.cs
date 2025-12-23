using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IItemMissaRepository
    {
        Task<List<ItemMissa>> ObterPorMissaIdAsync(int missaId);
        Task AdicionarAsync(ItemMissa item);
        Task AtualizarAsync(ItemMissa item);
        Task RemoverAsync(int id);
    }

}
