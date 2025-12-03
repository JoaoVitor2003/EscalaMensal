using EscalaMensal.Application.Services;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EscalaMensal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EscalaController : ControllerBase
    {
        private readonly IEscalaService _escalaService;

        public EscalaController(IEscalaService escalaService)
        {
            _escalaService = escalaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Escala>>> ObterTodas()
        {
            var escalas = await _escalaService.ObterTodasAsync();
            return Ok(escalas);
        }
        [HttpGet("id")]
        public async Task<ActionResult<Escala>> ObterPorId([FromQuery] int id)
        {
            var escala = await _escalaService.ObterPorIdAsync(id);
            if (escala == null)
                return NotFound();
            return Ok(escala);
        }

        [HttpGet("por-mes-ano")]
        public async Task<ActionResult<Escala>> ObterPorMesAno([FromQuery] int mes, [FromQuery] int ano)
        {
            var escala = await _escalaService.ObterPorMesAnoAsync(mes, ano);
            if (escala == null)
                return NotFound();

            return Ok(escala);
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar([FromBody] Escala novaEscala)
        {
            await _escalaService.AdicionarAsync(novaEscala);
            return CreatedAtAction(nameof(ObterPorMesAno), new { mes = novaEscala.Missas.FirstOrDefault()?.Dia.Month, ano = novaEscala.Missas.FirstOrDefault()?.Dia.Year }, novaEscala);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, [FromBody] Escala escala)
        {
            if (id != escala.Id)
                return BadRequest("ID da URL difere do corpo da requisição.");

            await _escalaService.AtualizarAsync(escala);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remover(int id)
        {
            await _escalaService.RemoverAsync(id);
            return NoContent();
        }
    }
}
