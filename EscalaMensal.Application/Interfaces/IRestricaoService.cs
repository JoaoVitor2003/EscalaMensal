using EscalaMensal.Application.DTOs.Restricao;
using EscalaMensal.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IRestricaoService
    {
        Task<List<RestricaoDto>> ObterPorMesAnoAsync(int mes, int ano);
        Task<List<RestricaoDto>> ObterPorMembroIdAsync(int membroId, int mes, int ano);
        Task AdicionarAsync(RestricaoAdicionarDto restricao);
        Task AtualizarAsync(RestricaoAtualizarDto restricao);
        Task RemoverAsync(int id);
    }
}
