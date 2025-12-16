using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IMissaService
    {
        Task<List<Missas>> ObterPorEscalaIdAsync(int escalaId);
        Task AdicionarAsync(Missas missa);
        Task AtualizarAsync(Missas missa);
        Task RemoverAsync(int id);
    }
}
