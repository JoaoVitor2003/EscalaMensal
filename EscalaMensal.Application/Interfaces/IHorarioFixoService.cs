using EscalaMensal.Application.DTOs.HorarioFixo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Interfaces
{
    public interface IHorarioFixoService
    {
        Task<List<HorarioFixoDto>> ObterTodosAsync();
        Task AdicionarAsync(HorarioFixoAdicionarDto dto);
        Task RemoverAsync(int id);
    }
}
