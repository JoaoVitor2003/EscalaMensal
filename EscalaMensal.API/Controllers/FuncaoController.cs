using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EscalaMensal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncaoController : ControllerBase
    {
        private readonly IFuncaoService _funcaoService;

        public FuncaoController(IFuncaoService funcaoService)
        {
            _funcaoService = funcaoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Funcao>>> ObterTodas()
        {
            return Ok(await _funcaoService.ObterTodasAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Funcao>> ObterPorId(int id)
        {
            var funcao = await _funcaoService.ObterPorIdAsync(id);
            if (funcao == null)
                return NotFound();

            return Ok(funcao);
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar(Funcao funcao)
        {
            await _funcaoService.AdicionarAsync(funcao);
            return CreatedAtAction(nameof(ObterPorId), new { id = funcao.Id }, funcao);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, Funcao funcao)
        {
            if (id != funcao.Id)
                return BadRequest("ID do parâmetro não confere com o da entidade");

            await _funcaoService.AtualizarAsync(funcao);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remover(int id)
        {
            await _funcaoService.RemoverAsync(id);
            return NoContent();
        }
    }
}
