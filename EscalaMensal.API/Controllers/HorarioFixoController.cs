using EscalaMensal.Application.DTOs.HorarioFixo;
using EscalaMensal.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorarioFixoController : ControllerBase
    {
        private readonly IHorarioFixoService _service;

        public HorarioFixoController(IHorarioFixoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<HorarioFixoDto>>> ObterTodos()
        {
            var horarios = await _service.ObterTodosAsync();
            return Ok(horarios);
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar([FromBody] HorarioFixoAdicionarDto dto)
        {
            await _service.AdicionarAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remover(int id)
        {
            await _service.RemoverAsync(id);
            return NoContent();
        }
    }
}
