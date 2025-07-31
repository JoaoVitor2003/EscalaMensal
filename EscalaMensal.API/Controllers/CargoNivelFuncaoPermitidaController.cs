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
        private readonly ICargoNivelFuncaoPermitidaRepository _repository;

        public CargoNivelFuncaoPermitidaController(ICargoNivelFuncaoPermitidaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("funcoes")]
        public async Task<IActionResult> ObterFuncoesPermitidas([FromQuery] CargoEnum cargo, [FromQuery] NivelEnum nivel)
        {
            var funcoes = await _repository.ObterFuncoesPermitidasAsync(cargo, nivel);
            return Ok(funcoes);
        }

        [HttpGet("todas")]
        public async Task<IActionResult> ObterTodas()
        {
            var todas = await _repository.ObterTodasAsync();
            return Ok(todas);
        }
    }
}
