using EscalaMensal.Application.DTOs.Usuario;
using EscalaMensal.Application.Interfaces;
using EscalaMensal.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
    }
}
