using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IHistoricoEscalaRepository
    {
        Task<List<HistoricoEscala>> ObterPorMesAnoAsync(int mes, int ano);
        Task<List<HistoricoEscala>> ObterPorUsuarioIdAsync(int usuarioId);
        Task AdicionarAsync(HistoricoEscala historico);
    }

}
