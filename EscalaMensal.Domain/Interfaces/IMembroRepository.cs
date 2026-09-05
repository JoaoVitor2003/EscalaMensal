using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IMembroRepository
    {
        Task<Membro?> ObterPorIdAsync(int id);
        Task<List<Membro>> ObterTodosAsync();
        Task AdicionarAsync(Membro membro);
        Task AtualizarAsync(Membro membro);
        Task RemoverAsync(int id);
    }
}
