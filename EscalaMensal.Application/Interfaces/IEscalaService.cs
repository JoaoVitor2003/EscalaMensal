using EscalaMensal.Application.DTOs.Escala;
using EscalaMensal.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IEscalaService
    {
        Task<EscalaDto?> ObterPorMesAnoAsync(int mes, int ano, int? usuarioId = null, int? perfil = null, int? paroquiaId = null);
        Task<List<EscalaDto>> ObterTodasAsync(int? usuarioId = null, int? perfil = null, int? paroquiaId = null);
        Task<EscalaDto?> ObterPorIdAsync(int id);
        Task AdicionarAsync(EscalaAdicionarDto escala);
        Task AtualizarAsync(EscalaAtualizarDto escala);
        Task RemoverAsync(int id);
        byte[] GerarPlanilhaExcel(EscalaDto escala);
    }
}
