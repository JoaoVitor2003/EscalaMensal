using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EscalaMensal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoricoEscalaController : ControllerBase
    {
        private readonly IHistoricoEscalaService _service;

        public HistoricoEscalaController(IHistoricoEscalaService service)
        {
            _service = service;
        }

        [HttpGet("por-mes-ano")]
        public async Task<IActionResult> ObterPorMesAno([FromQuery] int mes, [FromQuery] int ano)
        {
            var historicos = await _service.ObterPorMesAnoAsync(mes, ano);
            return Ok(historicos);
        }

        [HttpGet("por-usuario/{usuarioId}")]
        public async Task<IActionResult> ObterPorUsuarioId(int usuarioId)
        {
            var historicos = await _service.ObterPorUsuarioIdAsync(usuarioId);
            return Ok(historicos);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] HistoricoEscala historico)
        {
            await _service.AdicionarAsync(historico);
            return Ok();
        }
    }
}
