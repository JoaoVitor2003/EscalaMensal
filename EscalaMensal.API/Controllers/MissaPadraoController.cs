using EscalaMensal.Application.DTOs.MissaPadrao;
using EscalaMensal.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MissaPadraoController : ControllerBase
    {
        private readonly IMissaPadraoService _service;

        public MissaPadraoController(IMissaPadraoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<MissaPadraoDto>>> ObterTodas()
        {
            var missas = await _service.ObterTodasAsync();
            return Ok(missas);
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar([FromBody] MissaPadraoAdicionarDto dto)
        {
            await _service.AdicionarAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, [FromBody] MissaPadraoAdicionarDto dto)
        {
            await _service.AtualizarAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remover(int id)
        {
            await _service.RemoverAsync(id);
            return NoContent();
        }
    }
}
