using EscalaMensal.Application.DTOs.MissaPadrao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Interfaces
{
    public interface IMissaPadraoService
    {
        Task<List<MissaPadraoDto>> ObterTodasAsync();
        Task AdicionarAsync(MissaPadraoAdicionarDto dto);
        Task RemoverAsync(int id);
    }
}
