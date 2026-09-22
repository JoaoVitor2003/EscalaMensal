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
        private readonly ITokenService _tokenService;

        public UsuarioService(
            IUsuarioRepository usuarioRepository, 
            IPasswordHasher passwordHasher,
            ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
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

        public async Task<LoginRespostaDto> AutenticarAsync(LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Senha))
            {
                return new LoginRespostaDto
                {
                    Sucesso = false,
                    Mensagem = "E-mail e senha são obrigatórios."
                };
            }

            var usuario = await _usuarioRepository.ObterPorEmailAsync(dto.Email);
            if (usuario == null)
            {
                return new LoginRespostaDto
                {
                    Sucesso = false,
                    Mensagem = "E-mail ou senha incorretos."
                };
            }

            var senhaValida = _passwordHasher.VerifyPassword(dto.Senha, usuario.SenhaHash);
            if (!senhaValida)
            {
                return new LoginRespostaDto
                {
                    Sucesso = false,
                    Mensagem = "E-mail ou senha incorretos."
                };
            }

            // Validação de Status de Aprovação
            if (usuario.StatusAprovacao == Domain.Enums.StatusAprovacaoEnum.Pendente)
            {
                return new LoginRespostaDto
                {
                    Sucesso = false,
                    Status = usuario.StatusAprovacao,
                    Mensagem = "Seu cadastro ainda está aguardando aprovação do administrador. Por favor, aguarde a liberação para acessar o sistema."
                };
            }

            if (usuario.StatusAprovacao == Domain.Enums.StatusAprovacaoEnum.Rejeitado)
            {
                return new LoginRespostaDto
                {
                    Sucesso = false,
                    Status = usuario.StatusAprovacao,
                    Mensagem = "Sua solicitação de acesso não foi aprovada pela administração do sistema."
                };
            }

            // Usuário aprovado - gerar nova sessão única e persistir no banco (derrubando sessões anteriores)
            var novaSessaoId = Guid.NewGuid().ToString();
            usuario.IniciarNovaSessao(novaSessaoId);
            await _usuarioRepository.AtualizarAsync(usuario);

            // Gerar token JWT com a nova sessão vinculada
            var token = _tokenService.GerarToken(usuario);

            return new LoginRespostaDto
            {
                Sucesso = true,
                Token = token,
                Status = usuario.StatusAprovacao,
                Mensagem = "Login realizado com sucesso!",
                Usuario = new UsuarioRespostaDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Telefone = usuario.Telefone,
                    StatusAprovacao = usuario.StatusAprovacao,
                    Perfil = usuario.Perfil,
                    DataCriacao = usuario.DataCriacao
                }
            };
        }

        public async Task<bool> ValidarSessaoAtivaAsync(int usuarioId, string sessaoId)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            if (usuario == null)
                return false;

            return !string.IsNullOrEmpty(usuario.SessaoAtivaId) && usuario.SessaoAtivaId == sessaoId;
        }

        public async Task EncerrarSessaoAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            if (usuario != null)
            {
                usuario.EncerrarSessao();
                await _usuarioRepository.AtualizarAsync(usuario);
            }
        }
    }
}
