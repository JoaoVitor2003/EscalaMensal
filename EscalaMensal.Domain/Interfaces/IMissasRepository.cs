using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IMissasRepository
    {
        Task<Missas> ObterPorMissaIdAsync(int missaId);
        Task<bool> ExistePorDiaHorarioEscalaAsync(DateOnly dia, TimeOnly horario, int escalaId);
        Task AdicionarAsync(Missas missa);
        Task AtualizarAsync(Missas missa);
        Task RemoverAsync(int id);
    }
}
