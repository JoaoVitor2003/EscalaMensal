using EscalaMensal.Application.DTOs.Escala;
using EscalaMensal.Application.DTOs.Funcao;
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
        public async Task<ActionResult<List<FuncaoDto>>> ObterTodas([FromQuery] int? paroquiaId = null)
        {
            var funcoes = await _funcaoService.ObterTodasAsync(paroquiaId);
            return Ok(funcoes);
        }

        [HttpGet("modelos")]
        public async Task<ActionResult<List<FuncaoDto>>> ObterModelosBase()
        {
            var modelos = await _funcaoService.ObterModelosBaseAsync();
            return Ok(modelos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FuncaoDto>> ObterPorId(int id)
        {
            var funcao = await _funcaoService.ObterPorIdAsync(id);
            if (funcao == null)
                return NotFound();

            return Ok(funcao);
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar([FromBody]FuncaoAdicionarDto funcao)
        {
            await _funcaoService.AdicionarAsync(funcao);
            return Ok(funcao);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, FuncaoAtualizarDto funcao)
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
