using EscalaMensal.Application.DTOs.Usuario;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioRespostaDto> SolicitarAcessoAsync(SolicitarAcessoDto dto);
        Task<List<UsuarioRespostaDto>> ObterPendentesAsync();
        Task<UsuarioRespostaDto?> ObterPorIdAsync(int id);
        Task AprovarUsuarioAsync(int usuarioId, int aprovadorId);
        Task RejeitarUsuarioAsync(int usuarioId, int aprovadorId);
    }
}
