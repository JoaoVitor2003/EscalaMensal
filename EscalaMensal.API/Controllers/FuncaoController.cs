using AutoMapper;
using EscalaMensal.Application.DTOs.Funcao;
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
        private readonly IMapper _mapper;

        public FuncaoController(IFuncaoService funcaoService, IMapper mapper)
        {
            _funcaoService = funcaoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<FuncaoDto>>> ObterTodas()
        {
            var funcoes = await _funcaoService.ObterTodasAsync();
            var dto = _mapper.Map<List<FuncaoDto>>(funcoes);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FuncaoDto>> ObterPorId(int id)
        {
            var funcao = await _funcaoService.ObterPorIdAsync(id);
            if (funcao == null)
                return NotFound();

            var dto = _mapper.Map<FuncaoDto>(funcao);

            return Ok(dto);
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
