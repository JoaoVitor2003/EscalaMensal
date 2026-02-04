using AutoMapper;
using EscalaMensal.Application.DTOs.Escala;
using EscalaMensal.Application.Services;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EscalaMensal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EscalaController : ControllerBase
    {
        private readonly IEscalaService _escalaService;
        private readonly IMapper _mapper;

        public EscalaController(IEscalaService escalaService, IMapper mapper)
        {
            _escalaService = escalaService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<EscalaDto>>> ObterTodas()
        {
            var escalas = await _escalaService.ObterTodasAsync();

            var dto = _mapper.Map<List<EscalaDto>>(escalas);

            return Ok(dto);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<EscalaDto>> ObterPorId(int id)
        {
            var escala = await _escalaService.ObterPorIdAsync(id);
            if (escala == null)
                return NotFound();

            var dto = _mapper.Map<EscalaDto>(escala);
            return Ok(dto);
        }

        [HttpGet("por-mes-ano")]
        public async Task<ActionResult<Escala>> ObterPorMesAno([FromQuery] int mes, [FromQuery] int ano)
        {
            var escala = await _escalaService.ObterPorMesAnoAsync(mes, ano);
            if (escala == null)
                return NotFound();

            return Ok(escala);
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar([FromBody] EscalaAdicionarDto novaEscala)
        {
            var novaEscalaEntity = _mapper.Map<Escala>(novaEscala);
            await _escalaService.AdicionarAsync(novaEscalaEntity);
            return CreatedAtAction(nameof(ObterPorMesAno), new { mes = novaEscalaEntity.Missas.FirstOrDefault()?.Dia.Month, ano = novaEscalaEntity.Missas.FirstOrDefault()?.Dia.Year }, novaEscala);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, [FromBody] EscalaAtualizarDto escalaDto)
        {
            if (escalaDto == null) return BadRequest();

            var escalaExistente = await _escalaService.ObterPorIdAsync(id);
            if (escalaExistente == null) return NotFound("Escala não encontrada.");

            _mapper.Map(escalaDto, escalaExistente);

            await _escalaService.AtualizarAsync(escalaExistente);

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
