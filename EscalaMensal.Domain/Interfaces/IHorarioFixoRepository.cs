using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IHorarioFixoRepository
    {
        Task<HorarioFixo?> ObterPorIdAsync(int id);
        Task<List<HorarioFixo>> ObterTodosAsync();
        Task AdicionarAsync(HorarioFixo horario);
        Task RemoverAsync(int id);
    }
}
