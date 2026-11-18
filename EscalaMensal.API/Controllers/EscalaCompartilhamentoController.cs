using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EscalaMensal.API.Extensions;
using EscalaMensal.Application.DTOs.Escala;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Enums;
using EscalaMensal.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EscalaMensal.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EscalaCompartilhamentoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EscalaCompartilhamentoController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém os detalhes de compartilhamento de uma escala (código de acesso e usuários com permissão)
        /// </summary>
        [HttpGet("{escalaId}")]
        public async Task<IActionResult> ObterCompartilhamento(int escalaId)
        {
            var escala = await _context.Escalas.FindAsync(escalaId);
            if (escala == null)
                return NotFound(new { mensagem = "Escala não encontrada." });

            var compartilhamentos = await _context.EscalaCompartilhamentos
                .Include(c => c.Usuario)
                .Where(c => c.EscalaId == escalaId)
                .ToListAsync();

            // Encontra ou cria código de acesso geral
            var registroGeral = compartilhamentos.FirstOrDefault(c => c.UsuarioId == null);
            if (registroGeral == null)
            {
                var criadorId = await _context.Usuarios.Select(u => u.Id).FirstOrDefaultAsync();
                registroGeral = new EscalaCompartilhamento
                {
                    EscalaId = escalaId,
                    UsuarioId = null,
                    Permissao = 1, // Editor
                    CodigoAcesso = Guid.NewGuid().ToString("N")[..10].ToUpper(),
                    CriadoPorUsuarioId = criadorId,
                    DataCriacao = DateTime.UtcNow
                };

                _context.EscalaCompartilhamentos.Add(registroGeral);
                await _context.SaveChangesAsync();
                compartilhamentos.Add(registroGeral);
            }

            var usuariosCompartilhados = compartilhamentos
                .Where(c => c.UsuarioId != null)
                .Select(c => new EscalaCompartilhamentoDto
                {
                    Id = c.Id,
                    EscalaId = c.EscalaId,
                    UsuarioId = c.UsuarioId,
                    UsuarioNome = c.Usuario?.Nome,
                    UsuarioEmail = c.Usuario?.Email,
                    Permissao = c.Permissao,
                    CodigoAcesso = c.CodigoAcesso,
                    DataCriacao = c.DataCriacao
                })
                .ToList();

            return Ok(new
            {
                escalaId = escalaId,
                codigoAcesso = registroGeral.CodigoAcesso,
                usuarios = usuariosCompartilhados
            });
        }

        /// <summary>
        /// Compartilha a escala com um usuário específico (Exclusivo Administrador / Premium)
        /// </summary>
        [HttpPost("{escalaId}/compartilhar")]
        public async Task<IActionResult> CompartilharComUsuario(int escalaId, [FromBody] CompartilharUsuarioRequest request)
        {
            if (!User.EhPremiumOuAdmin())
            {
                return StatusCode(403, new { mensagem = "Apenas administradores e usuários com plano Premium podem compartilhar escalas." });
            }

            var escala = await _context.Escalas.FindAsync(escalaId);
            if (escala == null)
                return NotFound(new { mensagem = "Escala não encontrada." });

            var usuario = await _context.Usuarios.FindAsync(request.UsuarioId);
            if (usuario == null)
                return NotFound(new { mensagem = "Usuário não encontrado." });

            if (usuario.Perfil == PerfilUsuarioEnum.Normal)
            {
                return BadRequest(new { mensagem = "Apenas usuários com perfil Premium ou Administrador podem receber compartilhamento de escala." });
            }

            var criadorId = User.ObterUsuarioId();

            var existente = await _context.EscalaCompartilhamentos
                .FirstOrDefaultAsync(c => c.EscalaId == escalaId && c.UsuarioId == request.UsuarioId);

            if (existente != null)
            {
                existente.Permissao = request.Permissao;
                await _context.SaveChangesAsync();
                return Ok(new { mensagem = "Permissão do usuário atualizada com sucesso!" });
            }

            var novo = new EscalaCompartilhamento
            {
                EscalaId = escalaId,
                UsuarioId = request.UsuarioId,
                Permissao = request.Permissao,
                CodigoAcesso = Guid.NewGuid().ToString("N")[..10].ToUpper(),
                CriadoPorUsuarioId = criadorId,
                DataCriacao = DateTime.UtcNow
            };

            _context.EscalaCompartilhamentos.Add(novo);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Escala compartilhada com sucesso!" });
        }

        /// <summary>
        /// Remove o compartilhamento com um usuário
        /// </summary>
        [HttpDelete("{escalaId}/remover/{usuarioId}")]
        public async Task<IActionResult> RemoverCompartilhamento(int escalaId, int usuarioId)
        {
            if (!User.EhPremiumOuAdmin())
            {
                return StatusCode(403, new { mensagem = "Apenas administradores e usuários com plano Premium podem gerenciar compartilhamentos." });
            }

            var item = await _context.EscalaCompartilhamentos
                .FirstOrDefaultAsync(c => c.EscalaId == escalaId && c.UsuarioId == usuarioId);

            if (item == null)
                return NotFound(new { mensagem = "Compartilhamento não encontrado." });

            _context.EscalaCompartilhamentos.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Acesso removido com sucesso." });
        }

        /// <summary>
        /// Lista as escalas compartilhadas visíveis para o usuário autenticado
        /// (Admin vê todas que foram compartilhadas com alguém; usuário Premium só vê as compartilhadas com ele)
        /// </summary>
        [HttpGet("listar")]
        public async Task<ActionResult<List<EscalaCompartilhadaResumoDto>>> ListarEscalasCompartilhadas(
            [FromQuery] int? usuarioId = null, 
            [FromQuery] int? perfil = null)
        {
            var idUsuario = (User.EhAdmin() && usuarioId.HasValue && usuarioId.Value > 0) ? usuarioId.Value : User.ObterUsuarioId();
            var perfilUsuario = User.EhAdmin() ? PerfilUsuarioEnum.Administrador : User.ObterPerfil();

            // Usuários com perfil Normal não possuem acesso a escalas colaborativas
            if (perfilUsuario == PerfilUsuarioEnum.Normal)
            {
                return Ok(new List<EscalaCompartilhadaResumoDto>());
            }

            var queryCompartilhamentos = _context.EscalaCompartilhamentos
                .Include(c => c.Escala)
                .Include(c => c.Usuario)
                .Where(c => c.UsuarioId != null);

            var resultado = new List<EscalaCompartilhadaResumoDto>();

            if (perfilUsuario == PerfilUsuarioEnum.Administrador) // Administrador: vê todas as escalas compartilhadas com alguém
            {
                var grupos = await queryCompartilhamentos
                    .GroupBy(c => c.EscalaId)
                    .ToListAsync();

                foreach (var grupo in grupos)
                {
                    var escala = grupo.First().Escala;
                    if (escala == null) continue;

                    resultado.Add(new EscalaCompartilhadaResumoDto
                    {
                        EscalaId = escala.Id,
                        DataInicio = escala.DataInicio,
                        DataFim = escala.DataFim,
                        LimitePermitido = escala.LimitePermitido,
                        Permissao = "Editor",
                        TotalUsuarios = grupo.Count(),
                        NomesUsuariosCompartilhados = grupo.Select(g => g.Usuario?.Nome ?? "Usuário").Distinct().ToList(),
                        CodigoAcesso = grupo.FirstOrDefault()?.CodigoAcesso ?? string.Empty,
                        CompartilhadoPorMim = grupo.Any(g => g.CriadoPorUsuarioId == idUsuario),
                        DataCompartilhamento = grupo.Min(g => g.DataCriacao)
                    });
                }
            }
            else // Premium: vê apenas as compartilhadas com ele (ou criadas por ele)
            {
                var compartilhamentosUsuario = await queryCompartilhamentos
                    .Where(c => c.UsuarioId == idUsuario || c.CriadoPorUsuarioId == idUsuario)
                    .GroupBy(c => c.EscalaId)
                    .ToListAsync();

                foreach (var grupo in compartilhamentosUsuario)
                {
                    var escala = grupo.First().Escala;
                    if (escala == null) continue;

                    var meuRegistro = grupo.FirstOrDefault(g => g.UsuarioId == idUsuario);
                    string minhaPermissao = meuRegistro != null ? (meuRegistro.Permissao == 1 ? "Editor" : "Visualizador") : "Editor";

                    resultado.Add(new EscalaCompartilhadaResumoDto
                    {
                        EscalaId = escala.Id,
                        DataInicio = escala.DataInicio,
                        DataFim = escala.DataFim,
                        LimitePermitido = escala.LimitePermitido,
                        Permissao = minhaPermissao,
                        TotalUsuarios = grupo.Count(),
                        NomesUsuariosCompartilhados = grupo.Select(g => g.Usuario?.Nome ?? "Usuário").Distinct().ToList(),
                        CodigoAcesso = grupo.FirstOrDefault()?.CodigoAcesso ?? string.Empty,
                        CompartilhadoPorMim = grupo.Any(g => g.CriadoPorUsuarioId == idUsuario),
                        DataCompartilhamento = grupo.Min(g => g.DataCriacao)
                    });
                }
            }

            return Ok(resultado.OrderByDescending(r => r.DataInicio).ToList());
        }

        /// <summary>
        /// Retorna os IDs das escalas que possuem compartilhamento ativo para o usuário
        /// </summary>
        [HttpGet("ids-compartilhadas")]
        public async Task<ActionResult<List<int>>> ObterIdsEscalasCompartilhadas(
            [FromQuery] int? usuarioId = null, 
            [FromQuery] int? perfil = null)
        {
            var idUsuario = (User.EhAdmin() && usuarioId.HasValue && usuarioId.Value > 0) ? usuarioId.Value : User.ObterUsuarioId();
            var perfilUsuario = User.EhAdmin() ? PerfilUsuarioEnum.Administrador : User.ObterPerfil();

            if (perfilUsuario == PerfilUsuarioEnum.Normal)
            {
                return Ok(new List<int>());
            }

            if (perfilUsuario == PerfilUsuarioEnum.Administrador) // Administrador: todas que foram compartilhadas com alguém
            {
                var ids = await _context.EscalaCompartilhamentos
                    .Where(c => c.UsuarioId != null)
                    .Select(c => c.EscalaId)
                    .Distinct()
                    .ToListAsync();
                return Ok(ids);
            }
            else // Usuário Premium: apenas as compartilhadas com ele ou criadas por ele
            {
                var ids = await _context.EscalaCompartilhamentos
                    .Where(c => (c.UsuarioId == idUsuario || c.CriadoPorUsuarioId == idUsuario) && c.UsuarioId != null)
                    .Select(c => c.EscalaId)
                    .Distinct()
                    .ToListAsync();
                return Ok(ids);
            }
        }

        /// <summary>
        /// Valida se um usuário ou código tem acesso à escala compartilhada e retorna a permissão
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{escalaId}/validar")]
        public async Task<ActionResult<ValidarAcessoEscalaRespostaDto>> ValidarAcesso(
            int escalaId, 
            [FromQuery] string? codigo, 
            [FromQuery] int? usuarioId,
            [FromQuery] int? perfil)
        {
            var escala = await _context.Escalas.FindAsync(escalaId);
            if (escala == null)
                return NotFound(new ValidarAcessoEscalaRespostaDto { TemAcesso = false, Mensagem = "Escala não encontrada." });

            // Se autenticado via token, prioriza dados do token
            int? idFinal = User.Identity?.IsAuthenticated == true ? User.ObterUsuarioId() : usuarioId;
            int? perfilFinal = User.Identity?.IsAuthenticated == true ? (int)User.ObterPerfil() : perfil;

            // 0. Bloqueio imediato para usuário com perfil Normal (3)
            if (perfilFinal.HasValue && perfilFinal.Value == (int)PerfilUsuarioEnum.Normal)
            {
                return Ok(new ValidarAcessoEscalaRespostaDto
                {
                    TemAcesso = false,
                    Permissao = "Nenhum",
                    Mensagem = "Escalas ao vivo/compartilhadas são exclusivas para usuários com plano Premium ou Administrador."
                });
            }

            // Se usuarioId for fornecido, verificar no banco se o usuário é Normal
            if (idFinal.HasValue)
            {
                var usuario = await _context.Usuarios.FindAsync(idFinal.Value);
                if (usuario != null && usuario.Perfil == PerfilUsuarioEnum.Normal)
                {
                    return Ok(new ValidarAcessoEscalaRespostaDto
                    {
                        TemAcesso = false,
                        Permissao = "Nenhum",
                        Mensagem = "Escalas ao vivo/compartilhadas são exclusivas para usuários com plano Premium ou Administrador."
                    });
                }
            }

            // 1. Administrador (1) tem acesso total
            if (perfilFinal.HasValue && perfilFinal.Value == (int)PerfilUsuarioEnum.Administrador)
            {
                return Ok(new ValidarAcessoEscalaRespostaDto
                {
                    TemAcesso = true,
                    Permissao = "Editor",
                    Mensagem = "Acesso concedido como Administrador."
                });
            }

            // 2. Validação por Usuário cadastrado (convidado ou criador)
            if (idFinal.HasValue)
            {
                var compUsuario = await _context.EscalaCompartilhamentos
                    .FirstOrDefaultAsync(c => c.EscalaId == escalaId && (c.UsuarioId == idFinal.Value || c.CriadoPorUsuarioId == idFinal.Value));

                if (compUsuario != null)
                {
                    bool ehCriador = compUsuario.CriadoPorUsuarioId == idFinal.Value;
                    string permissaoNome = ehCriador || compUsuario.Permissao == 1 ? "Editor" : "Visualizador";
                    return Ok(new ValidarAcessoEscalaRespostaDto
                    {
                        TemAcesso = true,
                        Permissao = permissaoNome,
                        Mensagem = $"Acesso concedido como {permissaoNome}."
                    });
                }
            }

            // 3. Validação por Código de Acesso / Link Direto
            if (!string.IsNullOrWhiteSpace(codigo))
            {
                var compCodigo = await _context.EscalaCompartilhamentos
                    .FirstOrDefaultAsync(c => c.EscalaId == escalaId && c.CodigoAcesso == codigo.Trim().ToUpper());

                if (compCodigo != null)
                {
                    return Ok(new ValidarAcessoEscalaRespostaDto
                    {
                        TemAcesso = true,
                        Permissao = compCodigo.Permissao == 1 ? "Editor" : "Visualizador",
                        Mensagem = $"Acesso concedido via link como {(compCodigo.Permissao == 1 ? "Editor" : "Visualizador")}."
                    });
                }
            }

            return Ok(new ValidarAcessoEscalaRespostaDto
            {
                TemAcesso = false,
                Permissao = "Nenhum",
                Mensagem = "Você não tem permissão para acessar esta escala compartilhada. Requer convite de acesso do coordenador ou administrador."
            });
        }
    }
}
