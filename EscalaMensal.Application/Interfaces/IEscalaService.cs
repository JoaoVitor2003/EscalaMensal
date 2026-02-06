using EscalaMensal.Application.DTOs.Escala;
using EscalaMensal.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IEscalaService
    {
        Task<EscalaDto?> ObterPorMesAnoAsync(int mes, int ano);
        Task<List<EscalaDto>> ObterTodasAsync();
        Task<EscalaDto?> ObterPorIdAsync(int id);
        Task AdicionarAsync(EscalaAdicionarDto escala);
        Task AtualizarAsync(EscalaAtualizarDto escala);
        Task RemoverAsync(int id);
    }
}
