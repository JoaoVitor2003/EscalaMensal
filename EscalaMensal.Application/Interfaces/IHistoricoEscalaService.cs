using EscalaMensal.Application.DTOs.HistoricoEscala;
using EscalaMensal.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IHistoricoEscalaService
    {
        Task<List<HistoricoEscalaDto>> ObterPorMesAnoAsync(int mes, int ano);
        Task<List<HistoricoEscalaDto>> ObterPorMembroIdAsync(int membroId);
        Task AdicionarAsync(HistoricoEscala Adicionar);
        Task FinalizarEscalaAsync(int escalaId);
    }
}

