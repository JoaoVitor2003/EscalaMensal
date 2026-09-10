using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Enums;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscalaMensal.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObterPorIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.AprovadoPor)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            var normalizedEmail = email.Trim().ToLowerInvariant();
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);
        }

        public async Task<List<Usuario>> ObterPendentesAsync()
        {
            return await _context.Usuarios
                .Where(u => u.StatusAprovacao == StatusAprovacaoEnum.Pendente)
                .OrderBy(u => u.DataCriacao)
                .ToListAsync();
        }

        public async Task<List<Usuario>> ObterTodosAsync()
        {
            return await _context.Usuarios
                .OrderByDescending(u => u.DataCriacao)
                .ToListAsync();
        }

        public async Task AdicionarAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailJaExisteAsync(string email)
        {
            var normalizedEmail = email.Trim().ToLowerInvariant();
            return await _context.Usuarios
                .AnyAsync(u => u.Email.ToLower() == normalizedEmail);
        }
    }
}
