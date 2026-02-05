using EscalaMensal.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IFuncaoService
    {
        Task<List<Funcao>> ObterTodasAsync();
        Task<Funcao?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Funcao funcao);
        Task AtualizarAsync(Funcao funcao);
        Task RemoverAsync(int id);
    }
}
