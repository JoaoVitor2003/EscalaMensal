using EscalaMensal.Application.DTOs.Escala;
using EscalaMensal.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IEscalaService
    {
        Task<Escala?> ObterPorMesAnoAsync(int mes, int ano);
        Task<List<Escala>> ObterTodasAsync();
        Task<Escala?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Escala escala);
        Task AtualizarAsync(EscalaAtualizarDto escala);
        Task RemoverAsync(int id);
    }
}
