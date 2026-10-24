using EscalaMensal.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IParoquiaRepository
    {
        Task<List<Paroquia>> ObterTodasAsync();
        Task<Paroquia?> ObterPorIdAsync(int id);
        Task<Paroquia?> ObterPorCodigoAsync(string codigo);
        Task<List<Paroquia>> ObterPorUsuarioIdAsync(int usuarioId);
        Task<List<Paroquia>> BuscarPorTermoAsync(string termo);
        Task<UsuarioParoquia?> ObterVinculoUsuarioAsync(int usuarioId, int paroquiaId);
        Task AdicionarAsync(Paroquia paroquia);
        Task AtualizarAsync(Paroquia paroquia);
        Task AdicionarVinculoUsuarioAsync(UsuarioParoquia usuarioParoquia);
        Task AtualizarVinculoUsuarioAsync(UsuarioParoquia usuarioParoquia);
        Task RemoverVinculoUsuarioAsync(UsuarioParoquia usuarioParoquia);
        Task<List<SolicitacaoAcessoParoquia>> ObterSolicitacoesPorParoquiaIdAsync(int paroquiaId);
        Task<SolicitacaoAcessoParoquia?> ObterSolicitacaoPorIdAsync(int id);
        Task<SolicitacaoAcessoParoquia?> ObterSolicitacaoPendenteAsync(int usuarioId, int paroquiaId);
        Task AdicionarSolicitacaoAsync(SolicitacaoAcessoParoquia solicitacao);
        Task AtualizarSolicitacaoAsync(SolicitacaoAcessoParoquia solicitacao);
        Task AdicionarReivindicacaoAsync(ReivindicacaoParoquia reivindicacao);
        Task<List<ReivindicacaoParoquia>> ObterReivindicacoesPorParoquiaIdAsync(int paroquiaId);
    }
}
