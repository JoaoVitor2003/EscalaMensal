using EscalaMensal.Application.DTOs.Usuario;
using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDto?> ObterPorIdAsync(int id);
        Task<List<UsuarioDto>> ObterTodosAsync();
        Task AdicionarAsync(UsuarioAdicionarDto usuario);
        Task AtualizarAsync(UsuarioAtualizarDto usuario);
        Task RemoverAsync(int id);
    }
}
