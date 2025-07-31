using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EscalaMensal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItensEscalaController : ControllerBase
    {
        private readonly IItemEscalaService _itemEscalaService;

        public ItensEscalaController(IItemEscalaService itemEscalaService)
        {
            _itemEscalaService = itemEscalaService;
        }

        [HttpGet("por-escala/{escalaId}")]
        public async Task<ActionResult<List<ItemEscala>>> ObterPorEscalaId(int escalaId)
        {
            var itens = await _itemEscalaService.ObterPorEscalaIdAsync(escalaId);
            return Ok(itens);
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar([FromBody] ItemEscala item)
        {
            await _itemEscalaService.AdicionarAsync(item);
            return CreatedAtAction(nameof(ObterPorEscalaId), new { escalaId = item.EscalaId }, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, [FromBody] ItemEscala item)
        {
            if (id != item.Id)
                return BadRequest("O ID do item não confere.");

            await _itemEscalaService.AtualizarAsync(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remover(int id)
        {
            await _itemEscalaService.RemoverAsync(id);
            return NoContent();
        }
    }
}
