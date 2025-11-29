using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IEscalaRepository
    {
        Task<Escala?> ObterPorMesAnoAsync(int mes, int ano);
        Task<List<Escala>> ObterTodasAsync();
        Task<Escala?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Escala escala);
        Task AtualizarAsync(Escala escala);
        Task RemoverAsync(int id);
    }

}
