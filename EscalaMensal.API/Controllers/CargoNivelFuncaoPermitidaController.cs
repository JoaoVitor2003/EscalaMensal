using EscalaMensal.Application.DTOs.CargoNivel;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Enums;
using EscalaMensal.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EscalaMensal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CargoNivelFuncaoPermitidaController : ControllerBase
    {
        private readonly ICargoNivelFuncaoPermitidaService _service;

        public CargoNivelFuncaoPermitidaController(ICargoNivelFuncaoPermitidaService service)
        {
            _service = service;
        }

        [HttpGet("funcoes")]
        public async Task<IActionResult> ObterFuncoesPermitidas([FromQuery] CargoEnum cargo, [FromQuery] NivelEnum nivel)
        {
            var funcoes = await _service.ObterFuncoesPermitidasAsync(cargo, nivel);
            return Ok(funcoes);
        }

        [HttpGet("todas")]
        public async Task<IActionResult> ObterTodas()
        {
            var todas = await _service.ObterTodasAsync();
            return Ok(todas);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] CargoNivelAdicionarDto dto)
        {
            await _service.AdicionarAsync(dto.Cargo, dto.Nivel, dto.FuncaoId);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            await _service.RemoverAsync(id);
            return NoContent();
        }

        [HttpGet("max-nivel")]
        public async Task<IActionResult> ObterMaxNivel()
        {
            var max = await _service.ObterMaxNivelAsync();
            return Ok(max);
        }

        [HttpPost("max-nivel/incrementar")]
        public async Task<IActionResult> IncrementarNivel()
        {
            var max = await _service.IncrementarNivelAsync();
            return Ok(max);
        }

        [HttpPost("max-nivel/remover")]
        public async Task<IActionResult> RemoverNivelMaisAlto()
        {
            await _service.RemoverNivelMaisAltoAsync();
            return Ok();
        }

        [HttpPost("max-nivel/desfazer")]
        public async Task<IActionResult> DesfazerRemocaoNivel()
        {
            await _service.DesfazerRemocaoNivelAsync();
            return Ok();
        }
    }
}
