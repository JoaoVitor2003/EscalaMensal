using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EscalaMensal.Domain.Entities;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IMissaPadraoRepository
    {
        Task<List<MissaPadrao>> ObterTodasAsync();
        Task AdicionarAsync(MissaPadrao missaPadrao);
        Task AtualizarAsync(MissaPadrao missaPadrao);
        Task RemoverAsync(int id);
        Task<MissaPadrao?> ObterPorIdAsync(int id);
        Task<bool> ExistePorDiaHorarioAsync(DayOfWeek diaSemana, TimeOnly horario);
    }
}
