using EscalaMensal.Application.DTOs.Usuario;
using EscalaMensal.Application.Interfaces;
using EscalaMensal.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EscalaMensal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AuthController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Registra uma nova solicitação de acesso com senha criptografada de alta segurança.
        /// O status inicial fica como Pendente até a aprovação de um administrador.
        /// </summary>
        [HttpPost("solicitar-acesso")]
        public async Task<ActionResult<UsuarioRespostaDto>> SolicitarAcesso([FromBody] SolicitarAcessoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resultado = await _usuarioService.SolicitarAcessoAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Ocorreu um erro interno ao processar a solicitação de acesso.", detalhe = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioRespostaDto>> ObterPorId(int id)
        {
            var usuario = await _usuarioService.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound(new { mensagem = "Usuário não encontrado." });

            return Ok(usuario);
        }

        [HttpGet("pendentes")]
        public async Task<ActionResult<List<UsuarioRespostaDto>>> ObterPendentes()
        {
            var pendentes = await _usuarioService.ObterPendentesAsync();
            return Ok(pendentes);
        }

        [HttpGet("usuarios")]
        public async Task<ActionResult<List<UsuarioRespostaDto>>> ObterTodos()
        {
            var usuarios = await _usuarioService.ObterTodosAsync();
            return Ok(usuarios);
        }

        [HttpPost("{id}/aprovar")]
        public async Task<ActionResult> Aprovar(int id, [FromQuery] int aprovadorId = 1)
        {
            try
            {
                await _usuarioService.AprovarUsuarioAsync(id, aprovadorId);
                return Ok(new { mensagem = "Usuário aprovado com sucesso!" });
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPost("{id}/rejeitar")]
        public async Task<ActionResult> Rejeitar(int id, [FromQuery] int aprovadorId = 1)
        {
            try
            {
                await _usuarioService.RejeitarUsuarioAsync(id, aprovadorId);
                return Ok(new { mensagem = "Solicitação rejeitada com sucesso." });
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginRespostaDto>> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _usuarioService.AutenticarAsync(dto);

            if (!resultado.Sucesso)
            {
                if (resultado.Status.HasValue)
                {
                    return StatusCode(403, resultado);
                }

                return Unauthorized(resultado);
            }

            return Ok(resultado);
        }

        /// <summary>
        /// Valida se o token JWT apresentado corresponde à sessão ativa registrada no banco.
        /// Caso outro dispositivo tenha logado com a mesma conta, este endpoint indicará SessaoSubstituida.
        /// </summary>
        [HttpGet("validar-sessao")]
        public async Task<IActionResult> ValidarSessao()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized(new { ativa = false, mensagem = "Token ausente ou inválido." });
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            try
            {
                var handler = new JwtSecurityTokenHandler();
                if (!handler.CanReadToken(token))
                {
                    return Unauthorized(new { ativa = false, mensagem = "Formato de token inválido." });
                }

                var jwtToken = handler.ReadJwtToken(token);
                var idClaim = jwtToken.Claims.FirstOrDefault(c => 
                    c.Type == ClaimTypes.NameIdentifier || 
                    c.Type == "nameid" || 
                    c.Type == "sub")?.Value;
                var sessionClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "SessionId")?.Value;

                if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out var usuarioId) || string.IsNullOrEmpty(sessionClaim))
                {
                    return Unauthorized(new { ativa = false, mensagem = "Claims de sessão não encontradas no token." });
                }

                var sessaoValida = await _usuarioService.ValidarSessaoAtivaAsync(usuarioId, sessionClaim);
                if (!sessaoValida)
                {
                    return Ok(new { ativa = false, motivo = "SessaoSubstituida", mensagem = "Sua conta foi conectada em outro dispositivo ou navegador." });
                }

                return Ok(new { ativa = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ativa = false, mensagem = "Erro ao validar sessão.", detalhe = ex.Message });
            }
        }

        /// <summary>
        /// Encerra a sessão atual do usuário limpando a sessão ativa no banco de dados.
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return Ok(new { mensagem = "Logout efetuado." });
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            try
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    var idClaim = jwtToken.Claims.FirstOrDefault(c => 
                        c.Type == ClaimTypes.NameIdentifier || 
                        c.Type == "nameid" || 
                        c.Type == "sub")?.Value;

                    if (!string.IsNullOrEmpty(idClaim) && int.TryParse(idClaim, out var usuarioId))
                    {
                        await _usuarioService.EncerrarSessaoAsync(usuarioId);
                    }
                }
            }
            catch
            {
                // Ignora falha na leitura do token durante logout
            }

            return Ok(new { mensagem = "Sessão encerrada com sucesso." });
        }
    }
}
