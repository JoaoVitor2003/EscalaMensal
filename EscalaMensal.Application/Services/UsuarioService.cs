using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario?> ObterPorIdAsync(int id)
        {
            return await _usuarioRepository.ObterPorIdAsync(id);
        }

        public async Task<List<Usuario>> ObterTodosAsync()
        {
            return await _usuarioRepository.ObterTodosAsync();
        }

        public async Task AdicionarAsync(Usuario usuario)
        {
            await _usuarioRepository.AdicionarAsync(usuario);
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            await _usuarioRepository.AtualizarAsync(usuario);
        }

        public async Task RemoverAsync(int id)
        {
            await _usuarioRepository.RemoverAsync(id);
        }
    }
}
