using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IRestricaoRepository
    {
        Task<List<Restricao>> ObterPorMesAnoAsync(int mes, int ano);
        Task<List<Restricao>> ObterPorUsuarioIdAsync(int usuarioId, int mes, int ano);
        Task AdicionarAsync(Restricao restricao);
        Task AtualizarAsync(Restricao restricao);
        Task RemoverAsync(int id);
    }
}
