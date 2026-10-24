using EscalaMensal.Application.DTOs.Paroquia;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Interfaces
{
    public interface IParoquiaService
    {
        Task<List<ParoquiaDto>> ObterMinhasParoquiasAsync(int usuarioId);
        Task<List<ParoquiaResumoDto>> BuscarParoquiasAsync(string termo, int usuarioId);
        Task<List<ParoquiaDto>> ObterModelosAsync();
        Task<ParoquiaDto?> ObterPorIdAsync(int id, int? usuarioId = null);
        Task<ParoquiaDto> CriarAsync(ParoquiaCriarDto dto);
        Task<(bool Sucesso, string Mensagem, ParoquiaDto? Paroquia)> EntrarPorCodigoAsync(EntrarCodigoDto dto);
        Task<(bool Sucesso, string Mensagem)> SolicitarAcessoAsync(SolicitarAcessoParoquiaDto dto);
        Task<List<SolicitacaoAcessoParoquiaDto>> ObterSolicitacoesAsync(int paroquiaId);
        Task<(bool Sucesso, string Mensagem)> ResponderSolicitacaoAsync(int solicitacaoId, int respondidoPorUsuarioId, bool aprovar);
        Task<(bool Sucesso, string Mensagem)> ReivindicarAsync(ReivindicarParoquiaDto dto);
    }
}
