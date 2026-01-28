using AutoMapper;
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
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioDto>>> ObterTodos()
        {
            var usuarios = await _usuarioService.ObterTodosAsync();
            var dto = _mapper.Map<List<UsuarioDto>>(usuarios);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> ObterPorId(int id)
        {
            var usuario = await _usuarioService.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound();

            var dto = _mapper.Map<UsuarioDto>(usuario);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar([FromBody] Usuario usuario)
        {
            await _usuarioService.AdicionarAsync(usuario);
            return CreatedAtAction(nameof(ObterPorId), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id)
                return BadRequest("IDs não coincidem");

            await _usuarioService.AtualizarAsync(usuario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remover(int id)
        {
            await _usuarioService.RemoverAsync(id);
            return NoContent();
        }
    }
}
