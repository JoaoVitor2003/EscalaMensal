using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscalaMensal.Infrastructure.Repositories
{
    public class ParoquiaRepository : IParoquiaRepository
    {
        private readonly AppDbContext _context;

        public ParoquiaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Paroquia>> ObterTodasAsync()
        {
            return await _context.Paroquias
                .Include(p => p.CriadoPorUsuario)
                .Include(p => p.Membros)
                .ToListAsync();
        }

        public async Task<Paroquia?> ObterPorIdAsync(int id)
        {
            return await _context.Paroquias
                .Include(p => p.CriadoPorUsuario)
                .Include(p => p.UsuariosParoquia)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Paroquia?> ObterPorCodigoAsync(string codigo)
        {
            return await _context.Paroquias
                .FirstOrDefaultAsync(p => p.CodigoAcesso == codigo);
        }

        public async Task<List<Paroquia>> ObterPorUsuarioIdAsync(int usuarioId)
        {
            return await _context.UsuarioParoquias
                .Where(up => up.UsuarioId == usuarioId)
                .Include(up => up.Paroquia)
                    .ThenInclude(p => p.CriadoPorUsuario)
                .Select(up => up.Paroquia)
                .ToListAsync();
        }

        public async Task<List<Paroquia>> BuscarPorTermoAsync(string termo)
        {
            var termoLower = termo.ToLower().Trim();
            return await _context.Paroquias
                .Where(p => p.Ativa && (p.Nome.ToLower().Contains(termoLower) || p.Cidade.ToLower().Contains(termoLower) || (p.Bairro != null && p.Bairro.ToLower().Contains(termoLower))))
                .Include(p => p.Membros)
                .ToListAsync();
        }

        public async Task<UsuarioParoquia?> ObterVinculoUsuarioAsync(int usuarioId, int paroquiaId)
        {
            return await _context.UsuarioParoquias
                .FirstOrDefaultAsync(up => up.UsuarioId == usuarioId && up.ParoquiaId == paroquiaId);
        }

        public async Task AdicionarAsync(Paroquia paroquia)
        {
            await _context.Paroquias.AddAsync(paroquia);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Paroquia paroquia)
        {
            _context.Paroquias.Update(paroquia);
            await _context.SaveChangesAsync();
        }

        public async Task AdicionarVinculoUsuarioAsync(UsuarioParoquia usuarioParoquia)
        {
            await _context.UsuarioParoquias.AddAsync(usuarioParoquia);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarVinculoUsuarioAsync(UsuarioParoquia usuarioParoquia)
        {
            _context.UsuarioParoquias.Update(usuarioParoquia);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverVinculoUsuarioAsync(UsuarioParoquia usuarioParoquia)
        {
            _context.UsuarioParoquias.Remove(usuarioParoquia);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SolicitacaoAcessoParoquia>> ObterSolicitacoesPorParoquiaIdAsync(int paroquiaId)
        {
            return await _context.SolicitacoesAcessoParoquia
                .Include(s => s.Usuario)
                .Include(s => s.Paroquia)
                .Include(s => s.RespondidoPorUsuario)
                .Where(s => s.ParoquiaId == paroquiaId)
                .OrderByDescending(s => s.DataSolicitacao)
                .ToListAsync();
        }

        public async Task<SolicitacaoAcessoParoquia?> ObterSolicitacaoPorIdAsync(int id)
        {
            return await _context.SolicitacoesAcessoParoquia
                .Include(s => s.Usuario)
                .Include(s => s.Paroquia)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<SolicitacaoAcessoParoquia?> ObterSolicitacaoPendenteAsync(int usuarioId, int paroquiaId)
        {
            return await _context.SolicitacoesAcessoParoquia
                .FirstOrDefaultAsync(s => s.UsuarioId == usuarioId && s.ParoquiaId == paroquiaId && s.Status == 1);
        }

        public async Task AdicionarSolicitacaoAsync(SolicitacaoAcessoParoquia solicitacao)
        {
            await _context.SolicitacoesAcessoParoquia.AddAsync(solicitacao);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarSolicitacaoAsync(SolicitacaoAcessoParoquia solicitacao)
        {
            _context.SolicitacoesAcessoParoquia.Update(solicitacao);
            await _context.SaveChangesAsync();
        }

        public async Task AdicionarReivindicacaoAsync(ReivindicacaoParoquia reivindicacao)
        {
            await _context.ReivindicacoesParoquia.AddAsync(reivindicacao);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ReivindicacaoParoquia>> ObterReivindicacoesPorParoquiaIdAsync(int paroquiaId)
        {
            return await _context.ReivindicacoesParoquia
                .Include(r => r.Usuario)
                .Include(r => r.Paroquia)
                .Where(r => r.ParoquiaId == paroquiaId)
                .OrderByDescending(r => r.DataSolicitacao)
                .ToListAsync();
        }
    }
}
