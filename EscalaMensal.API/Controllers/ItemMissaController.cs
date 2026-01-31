using AutoMapper;
using EscalaMensal.Application.DTOs.ItemMissa;
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
        private readonly IMapper _mapper;

        public ItensMissaController(IItemMissaService itemMissaService, IMapper mapper)
        {
            _itemMissaService = itemMissaService;
            _mapper = mapper;
        }

        [HttpGet("por-escala/{missaId}")]
        public async Task<ActionResult<List<ItemMissaDto>>> ObterPorMissaIdAsync(int missaId)
        {
            var itens = await _itemMissaService.ObterPorMissaIdAsync(missaId);
            if (itens == null) return NotFound();

            var dto = _mapper.Map<List<ItemMissaDto>>(itens);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar([FromBody] ItemMissaAdicionarDto item)
        {
            var itemMissaEntity = _mapper.Map<ItemMissa>(item);
            await _itemMissaService.AdicionarAsync(itemMissaEntity);
            return Ok(itemMissaEntity);
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
