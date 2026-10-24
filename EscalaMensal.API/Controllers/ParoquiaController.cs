using EscalaMensal.Application.DTOs.Paroquia;
using EscalaMensal.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParoquiaController : ControllerBase
    {
        private readonly IParoquiaService _paroquiaService;

        public ParoquiaController(IParoquiaService paroquiaService)
        {
            _paroquiaService = paroquiaService;
        }

        [HttpGet("minhas")]
        public async Task<ActionResult<List<ParoquiaDto>>> ObterMinhas([FromQuery] int usuarioId)
        {
            var paroquias = await _paroquiaService.ObterMinhasParoquiasAsync(usuarioId);
            return Ok(paroquias);
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<List<ParoquiaResumoDto>>> Buscar([FromQuery] string termo, [FromQuery] int usuarioId)
        {
            var paroquias = await _paroquiaService.BuscarParoquiasAsync(termo, usuarioId);
            return Ok(paroquias);
        }

        [HttpGet("modelos")]
        public async Task<ActionResult<List<ParoquiaDto>>> ObterModelos()
        {
            var modelos = await _paroquiaService.ObterModelosAsync();
            return Ok(modelos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParoquiaDto>> ObterPorId(int id, [FromQuery] int? usuarioId = null)
        {
            var paroquia = await _paroquiaService.ObterPorIdAsync(id, usuarioId);
            if (paroquia == null)
                return NotFound("Paróquia não encontrada.");

            return Ok(paroquia);
        }

        [HttpPost]
        public async Task<ActionResult<ParoquiaDto>> Criar([FromBody] ParoquiaCriarDto dto)
        {
            var novaParoquia = await _paroquiaService.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = novaParoquia.Id }, novaParoquia);
        }

        [HttpPost("entrar-codigo")]
        public async Task<ActionResult> EntrarPorCodigo([FromBody] EntrarCodigoDto dto)
        {
            var resultado = await _paroquiaService.EntrarPorCodigoAsync(dto);
            if (!resultado.Sucesso)
            {
                return BadRequest(new { mensagem = resultado.Mensagem });
            }

            return Ok(new { mensagem = resultado.Mensagem, paroquia = resultado.Paroquia });
        }

        [HttpPost("solicitar-acesso")]
        public async Task<ActionResult> SolicitarAcesso([FromBody] SolicitarAcessoParoquiaDto dto)
        {
            var resultado = await _paroquiaService.SolicitarAcessoAsync(dto);
            if (!resultado.Sucesso)
            {
                return BadRequest(new { mensagem = resultado.Mensagem });
            }

            return Ok(new { mensagem = resultado.Mensagem });
        }

        [HttpGet("{id}/solicitacoes")]
        public async Task<ActionResult<List<SolicitacaoAcessoParoquiaDto>>> ObterSolicitacoes(int id)
        {
            var solicitacoes = await _paroquiaService.ObterSolicitacoesAsync(id);
            return Ok(solicitacoes);
        }

        [HttpPost("solicitacoes/{id}/aprovar")]
        public async Task<ActionResult> AprovarSolicitacao(int id, [FromQuery] int respondidoPorUsuarioId)
        {
            var resultado = await _paroquiaService.ResponderSolicitacaoAsync(id, respondidoPorUsuarioId, true);
            if (!resultado.Sucesso)
            {
                return BadRequest(new { mensagem = resultado.Mensagem });
            }

            return Ok(new { mensagem = resultado.Mensagem });
        }

        [HttpPost("solicitacoes/{id}/rejeitar")]
        public async Task<ActionResult> RejeitarSolicitacao(int id, [FromQuery] int respondidoPorUsuarioId)
        {
            var resultado = await _paroquiaService.ResponderSolicitacaoAsync(id, respondidoPorUsuarioId, false);
            if (!resultado.Sucesso)
            {
                return BadRequest(new { mensagem = resultado.Mensagem });
            }

            return Ok(new { mensagem = resultado.Mensagem });
        }

        [HttpPost("reivindicar")]
        public async Task<ActionResult> Reivindicar([FromBody] ReivindicarParoquiaDto dto)
        {
            var resultado = await _paroquiaService.ReivindicarAsync(dto);
            if (!resultado.Sucesso)
            {
                return BadRequest(new { mensagem = resultado.Mensagem });
            }

            return Ok(new { mensagem = resultado.Mensagem });
        }
    }
}
