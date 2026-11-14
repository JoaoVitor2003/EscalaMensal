using EscalaMensal.API.Extensions;
using EscalaMensal.Application.DTOs.Escala;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscalaMensal.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EscalaController : ControllerBase
    {
        private readonly IEscalaService _escalaService;

        public EscalaController(IEscalaService escalaService)
        {
            _escalaService = escalaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<EscalaDto>>> ObterTodas([FromQuery] int? usuarioId, [FromQuery] int? perfil, [FromQuery] int? paroquiaId = null)
        {
            var idAutenticado = User.ObterUsuarioId();
            var perfilAutenticado = User.ObterPerfil();

            // Administrador pode consultar qualquer usuário; usuários comuns consultam apenas o próprio ID
            var idEfetivo = User.EhAdmin() && usuarioId.HasValue ? usuarioId.Value : idAutenticado;
            var perfilEfetivo = User.EhAdmin() && perfil.HasValue ? perfil.Value : (int)perfilAutenticado;

            var escalas = await _escalaService.ObterTodasAsync(idEfetivo, perfilEfetivo, paroquiaId);

            return Ok(escalas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EscalaDto>> ObterPorId(int id)
        {
            var escala = await _escalaService.ObterPorIdAsync(id);
            if (escala == null)
                return NotFound();
            return Ok(escala);
        }

        [HttpGet("por-mes-ano")]
        public async Task<ActionResult<EscalaDto>> ObterPorMesAno([FromQuery] int mes, [FromQuery] int ano, [FromQuery] int? usuarioId, [FromQuery] int? perfil, [FromQuery] int? paroquiaId = null)
        {
            var idAutenticado = User.ObterUsuarioId();
            var perfilAutenticado = User.ObterPerfil();

            var idEfetivo = User.EhAdmin() && usuarioId.HasValue ? usuarioId.Value : idAutenticado;
            var perfilEfetivo = User.EhAdmin() && perfil.HasValue ? perfil.Value : (int)perfilAutenticado;

            var escala = await _escalaService.ObterPorMesAnoAsync(mes, ano, idEfetivo, perfilEfetivo, paroquiaId);
            if (escala == null)
                return NotFound();

            return Ok(escala);
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar([FromBody] EscalaAdicionarDto novaEscala)
        {
            // Vincula o criador ao usuário autenticado de forma confiável
            if (!novaEscala.CriadoPorUsuarioId.HasValue || !User.EhAdmin())
            {
                novaEscala.CriadoPorUsuarioId = User.ObterUsuarioId();
            }

            await _escalaService.AdicionarAsync(novaEscala);
            return Ok(novaEscala);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, [FromBody] EscalaAtualizarDto escalaDto)
        {
            escalaDto.Id = id;
            await _escalaService.AtualizarAsync(escalaDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remover(int id, [FromQuery] int? usuarioId, [FromQuery] int? perfil)
        {
            var escala = await _escalaService.ObterPorIdAsync(id);
            if (escala == null)
                return NotFound();

            var idAutenticado = User.ObterUsuarioId();
            var ehAdmin = User.EhAdmin();

            if (!ehAdmin && escala.CriadoPorUsuarioId.HasValue && escala.CriadoPorUsuarioId.Value != idAutenticado)
            {
                return Forbid();
            }

            await _escalaService.RemoverAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/exportar-excel")]
        public async Task<IActionResult> ExportarExcel(int id)
        {
            var escala = await _escalaService.ObterPorIdAsync(id);
            if (escala == null)
                return NotFound();

            byte[] fileBytes = _escalaService.GerarPlanilhaExcel(escala);
            var fileName = $"escala_{escala.DataInicio:yyyy_MM_dd}_a_{escala.DataFim:yyyy_MM_dd}.xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        /// <summary>
        /// Gera automaticamente e de forma equilibrada as alocações da escala (Recurso Exclusivo Premium / Administrador).
        /// Protegido por Token JWT e Claims autorizadas.
        /// </summary>
        [Authorize(Roles = "Administrador,Premium")]
        [HttpPost("{id}/gerar-aleatorio")]
        public async Task<ActionResult<GerarEscalaAleatoriaResultadoDto>> GerarAleatorio(
            int id, 
            [FromBody] GerarEscalaAleatoriaRequestDto request)
        {
            if (!User.EhPremiumOuAdmin())
            {
                return StatusCode(403, new GerarEscalaAleatoriaResultadoDto
                {
                    Sucesso = false,
                    Mensagem = "Recurso exclusivo do Plano Premium. Atualize seu plano para gerar escalas automaticamente!"
                });
            }

            // Garante que o ID e o Perfil enviados ao Service venham dos Claims verificados
            request.UsuarioId = User.ObterUsuarioId();
            request.Perfil = (int)User.ObterPerfil();

            var resultado = await _escalaService.GerarEscalaAleatoriaAsync(id, request);
            if (!resultado.Sucesso)
            {
                return BadRequest(resultado);
            }

            return Ok(resultado);
        }
    }
}
