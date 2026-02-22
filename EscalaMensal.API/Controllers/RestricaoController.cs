using AutoMapper;
using EscalaMensal.Application.DTOs.Escala;
using EscalaMensal.Application.DTOs.Restricao;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EscalaMensal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestricaoController : ControllerBase
    {
        private readonly IRestricaoService _restricaoService;
        private readonly IMapper _mapper;

        public RestricaoController(IRestricaoService restricaoService, IMapper mapper)
        {
            _restricaoService = restricaoService;
            _mapper = mapper;
        }

        [HttpGet("mes/{mes}/ano/{ano}")]
        public async Task<IActionResult> ObterPorMesAno(int mes, int ano)
        {
            var restricoes = await _restricaoService.ObterPorMesAnoAsync(mes, ano);
            return Ok(restricoes);
        }

        [HttpGet("usuario/{usuarioId}/mes/{mes}/ano/{ano}")]
        public async Task<IActionResult> ObterPorUsuario(int usuarioId, int mes, int ano)
        {
            var restricoes = await _restricaoService.ObterPorUsuarioIdAsync(usuarioId, mes, ano);
            return Ok(restricoes);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] RestricaoAdicionarDto restricao)
        {
            await _restricaoService.AdicionarAsync(restricao);
            return Ok(restricao);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] RestricaoAtualizarDto restricao)
        {
            if (id != restricao.Id)
                return BadRequest("IDs não correspondem");

            await _restricaoService.AtualizarAsync(restricao);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(RestricaoDeleteDto dto)
        {
            var id = dto.Id;
            await _restricaoService.RemoverAsync(id);
            return NoContent();
        }
    }
}
