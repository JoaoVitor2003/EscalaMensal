using AutoMapper;
using EscalaMensal.Application.DTOs.Missa;
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
        private readonly IMapper _mapper;
        public MissasController(IMissasService missasService, IMapper mapper)
        {
            _missasService = missasService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MissasDto>> ObterPorEscalaId(int id)
        {
            var missas = await _missasService.ObterPorMissaIdAsync(id);

            if (missas == null)
                return NotFound();

            var dto = _mapper.Map<MissasDto>(missas);
            return Ok(dto);
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
