using EscalaMensal.Application.DTOs.Usuario;
using EscalaMensal.Application.Interfaces;
using EscalaMensal.Application.Security;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Exceptions;
using EscalaMensal.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UsuarioService(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UsuarioRespostaDto> SolicitarAcessoAsync(SolicitarAcessoDto dto)
        {
            var emailJaCadastrado = await _usuarioRepository.EmailJaExisteAsync(dto.Email);
            if (emailJaCadastrado)
            {
                throw new DomainException("Já existe um cadastro ou solicitação de acesso com este endereço de e-mail.");
            }

            // Criptografia com algoritmo robusto (BCrypt + salt individual + SHA-384)
            var senhaCriptografada = _passwordHasher.HashPassword(dto.Senha);

            var usuario = new Usuario(
                nome: dto.Nome.Trim(),
                email: dto.Email.Trim().ToLowerInvariant(),
                telefone: dto.Telefone.Trim(),
                senhaHash: senhaCriptografada
            );

            await _usuarioRepository.AdicionarAsync(usuario);

            return new UsuarioRespostaDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                StatusAprovacao = usuario.StatusAprovacao,
                Perfil = usuario.Perfil,
                DataCriacao = usuario.DataCriacao
            };
        }

        public async Task<List<UsuarioRespostaDto>> ObterPendentesAsync()
        {
            var pendentes = await _usuarioRepository.ObterPendentesAsync();
            return pendentes.Select(u => new UsuarioRespostaDto
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Telefone = u.Telefone,
                StatusAprovacao = u.StatusAprovacao,
                Perfil = u.Perfil,
                DataCriacao = u.DataCriacao
            }).ToList();
        }

        public async Task<List<UsuarioRespostaDto>> ObterTodosAsync()
        {
            var usuarios = await _usuarioRepository.ObterTodosAsync();
            return usuarios.Select(u => new UsuarioRespostaDto
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Telefone = u.Telefone,
                StatusAprovacao = u.StatusAprovacao,
                Perfil = u.Perfil,
                DataCriacao = u.DataCriacao
            }).ToList();
        }

        public async Task<UsuarioRespostaDto?> ObterPorIdAsync(int id)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);
            if (usuario == null) return null;

            return new UsuarioRespostaDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                StatusAprovacao = usuario.StatusAprovacao,
                Perfil = usuario.Perfil,
                DataCriacao = usuario.DataCriacao
            };
        }

        public async Task AprovarUsuarioAsync(int usuarioId, int aprovadorId)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            if (usuario == null)
                throw new DomainException("Usuário não encontrado.");

            usuario.Aprovar(aprovadorId);
            await _usuarioRepository.AtualizarAsync(usuario);
        }

        public async Task RejeitarUsuarioAsync(int usuarioId, int aprovadorId)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            if (usuario == null)
                throw new DomainException("Usuário não encontrado.");

            usuario.Rejeitar(aprovadorId);
            await _usuarioRepository.AtualizarAsync(usuario);
        }
    }
}
