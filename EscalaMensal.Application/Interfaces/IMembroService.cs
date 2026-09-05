using EscalaMensal.Application.DTOs.Membro;
using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IMembroService
    {
        Task<MembroDto?> ObterPorIdAsync(int id);
        Task<List<MembroDto>> ObterTodosAsync();
        Task AdicionarAsync(MembroAdicionarDto membro);
        Task AtualizarAsync(MembroAtualizarDto membro);
        Task RemoverAsync(int id);
    }
}
