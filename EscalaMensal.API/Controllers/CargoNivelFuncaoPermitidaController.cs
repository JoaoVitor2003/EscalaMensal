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
    }
}
