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
        Task<ItemMissa?> ObterPorIdAsync(int id);
        Task<ItemMissa> ObterPorMissaIdAsync(int missaId);
        Task<bool> ExisteMembroNaMissaAsync(int missaId, int membroId);
        Task<bool> ExisteFuncaoNaMissaAsync(int missaId, int funcaoId);
        Task<int> QuantidadeDeEscalasDoMembroNaEscalaAsync(int escalaId, int membroId);
        Task AdicionarAsync(ItemMissa item);
        Task AtualizarOrdemItensMissa(List<ItemMissa> itens);
        Task AtualizarAsync(ItemMissa item);
        Task RemoverAsync(int id);
    }

}
