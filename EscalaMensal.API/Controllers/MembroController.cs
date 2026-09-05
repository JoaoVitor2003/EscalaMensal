using AutoMapper;
using EscalaMensal.Application.DTOs.Escala;
using EscalaMensal.Application.DTOs.Membro;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EscalaMensal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/usuario")]
    public class MembroController : ControllerBase
    {
        private readonly IMembroService _membroService;

        public MembroController(IMembroService membroService)
        {
            _membroService = membroService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MembroDto>>> ObterTodos()
        {
            var membros = await _membroService.ObterTodosAsync();
            return Ok(membros);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MembroDto>> ObterPorId(int id)
        {
            var membro = await _membroService.ObterPorIdAsync(id);
            if (membro == null)
                return NotFound();

            return Ok(membro);
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar([FromBody] MembroAdicionarDto membro)
        {
            await _membroService.AdicionarAsync(membro);
            return Ok(membro);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, [FromBody] MembroAtualizarDto membro)
        {
            if (id != membro.Id)
                return BadRequest("IDs não coincidem");

            await _membroService.AtualizarAsync(membro);
            var membroAtualizado = await _membroService.ObterPorIdAsync(id);
            return Ok(membroAtualizado);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remover(int id)
        {
            await _membroService.RemoverAsync(id);
            return NoContent();
        }
    }
}
