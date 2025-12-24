using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EscalaMensal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItensMissaController : ControllerBase
    {
        private readonly IItemMissaService _itemMissaService;

        public ItensMissaController(IItemMissaService itemMissaService)
        {
            _itemMissaService = itemMissaService;
        }

        [HttpGet("por-escala/{missaId}")]
        public async Task<ActionResult<List<ItemMissa>>> ObterPorMissaIdAsync(int missaId)
        {
            var itens = await _itemMissaService.ObterPorMissaIdAsync(missaId);
            if (itens == null) return NotFound();
            return Ok(itens);
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar([FromBody] ItemMissa item)
        {
            await _itemMissaService.AdicionarAsync(item);
            return CreatedAtAction(nameof(ObterPorMissaIdAsync), new { missaId = item.MissaId }, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, [FromBody] ItemMissa item)
        {
            if (id != item.Id)
                return BadRequest("O ID do item não confere.");

            await _itemMissaService.AtualizarAsync(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remover(int id)
        {
            await _itemMissaService.RemoverAsync(id);
            return NoContent();
        }
    }
}
