using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EscalaMensal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MissasController : Controller
    {
        private readonly IMissasService _missasService;
        public MissasController(IMissasService missasService)
        {
            _missasService = missasService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Missas>>> ObterPorEscalaId(int id)
        {
            return Ok(await _missasService.ObterPorEscalaIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar([FromBody] Missas missa)
        {
            await _missasService.AdicionarAsync(missa);
            return CreatedAtAction(nameof(ObterPorEscalaId), new { id = missa.EscalaId }, missa);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, Missas missa)
        {
            if (id != missa.Id)
                return BadRequest("ID do parâmetro não confere com o da entidade");
            await _missasService.AtualizarAsync(missa);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remover(int id)
        {
            await _missasService.RemoverAsync(id);
            return NoContent();
        }
    }
}
