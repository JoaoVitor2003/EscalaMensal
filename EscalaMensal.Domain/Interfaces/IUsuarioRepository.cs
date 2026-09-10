using EscalaMensal.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterPorIdAsync(int id);
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task<List<Usuario>> ObterPendentesAsync();
        Task<List<Usuario>> ObterTodosAsync();
        Task AdicionarAsync(Usuario usuario);
        Task AtualizarAsync(Usuario usuario);
        Task<bool> EmailJaExisteAsync(string email);
    }
}
