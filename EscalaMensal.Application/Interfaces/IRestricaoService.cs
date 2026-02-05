using EscalaMensal.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IRestricaoService
    {
        Task<List<Restricao>> ObterPorMesAnoAsync(int mes, int ano);
        Task<List<Restricao>> ObterPorUsuarioIdAsync(int usuarioId, int mes, int ano);
        Task AdicionarAsync(Restricao restricao);
        Task AtualizarAsync(Restricao restricao);
        Task RemoverAsync(int id);
    }
}
