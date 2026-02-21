using EscalaMensal.Application.DTOs.Funcao;
using EscalaMensal.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IFuncaoService
    {
        Task<List<FuncaoDto>> ObterTodasAsync();
        Task<FuncaoDto?> ObterPorIdAsync(int id);
        Task AdicionarAsync(FuncaoAdicionarDto funcao);
        Task AtualizarAsync(FuncaoAtualizarDto funcao);
        Task RemoverAsync(int id);
    }
}
