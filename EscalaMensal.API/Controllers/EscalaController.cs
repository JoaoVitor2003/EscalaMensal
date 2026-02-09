using EscalaMensal.Application.DTOs.Escala;
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
        public async Task<ActionResult<List<EscalaDto>>> ObterTodas()
        {
            var escalas = await _escalaService.ObterTodasAsync();


            return Ok(escalas);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<EscalaDto>> ObterPorId(int id)
        {
            var escala = await _escalaService.ObterPorIdAsync(id);
            if (escala == null)
                return NotFound();
            return Ok(escala);
        }

        [HttpGet("por-mes-ano")]
        public async Task<ActionResult<EscalaDto>> ObterPorMesAno([FromQuery] int mes, [FromQuery] int ano)
        {
            var escala = await _escalaService.ObterPorMesAnoAsync(mes, ano);
            if (escala == null)
                return NotFound();

            return Ok(escala);
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar([FromBody] EscalaAdicionarDto novaEscala)
        {
            await _escalaService.AdicionarAsync(novaEscala);
            return Ok(novaEscala);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, [FromBody] EscalaAtualizarDto escalaDto)
        {
            escalaDto.Id = id;
            await _escalaService.AtualizarAsync(escalaDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remover(EscalaDeleteDto dto)
        {
            var id = dto.Id;
            await _escalaService.RemoverAsync(id);
            return NoContent();
        }
    }
}
