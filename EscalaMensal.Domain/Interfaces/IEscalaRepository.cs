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
        Task<Escala?> ObterPorMesAnoAsync(int mes, int ano, int? usuarioId = null, int? perfil = null);
        Task<List<Escala>> ObterTodasAsync(int? usuarioId = null, int? perfil = null);
        Task<Escala?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Escala escala);
        Task AtualizarAsync(Escala escala);
        Task RemoverAsync(int id);
    }

}
