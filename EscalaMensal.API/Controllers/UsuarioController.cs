using AutoMapper;
using EscalaMensal.Application.DTOs.Escala;
using EscalaMensal.Application.DTOs.Usuario;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EscalaMensal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioDto>>> ObterTodos()
        {
            var usuarios = await _usuarioService.ObterTodosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> ObterPorId(int id)
        {
            var usuario = await _usuarioService.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar([FromBody] UsuarioAdicionarDto usuario)
        {
            await _usuarioService.AdicionarAsync(usuario);
            return Ok(usuario);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, [FromBody] UsuarioAtualizarDto usuario)
        {
            if (id != usuario.Id)
                return BadRequest("IDs não coincidem");

            await _usuarioService.AtualizarAsync(usuario);
            var usuarioAtualizado = await _usuarioService.ObterPorIdAsync(id);
            return Ok(usuarioAtualizado);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remover(UsuarioDeleteDto dto)
        {
            var id = dto.Id;
            await _usuarioService.RemoverAsync(id);
            return NoContent();
        }
    }
}
