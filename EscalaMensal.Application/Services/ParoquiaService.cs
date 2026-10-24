using AutoMapper;
using EscalaMensal.Application.DTOs.Paroquia;
using EscalaMensal.Application.Interfaces;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Exceptions;
using EscalaMensal.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class ParoquiaService : IParoquiaService
    {
        private readonly IParoquiaRepository _paroquiaRepository;
        private readonly IFuncaoRepository _funcaoRepository;
        private readonly IMapper _mapper;

        public ParoquiaService(
            IParoquiaRepository paroquiaRepository,
            IFuncaoRepository funcaoRepository,
            IMapper mapper)
        {
            _paroquiaRepository = paroquiaRepository;
            _funcaoRepository = funcaoRepository;
            _mapper = mapper;
        }

        public async Task<List<ParoquiaDto>> ObterMinhasParoquiasAsync(int usuarioId)
        {
            var paroquias = await _paroquiaRepository.ObterPorUsuarioIdAsync(usuarioId);
            var dtos = _mapper.Map<List<ParoquiaDto>>(paroquias);
            foreach (var dto in dtos)
            {
                dto.EhCriador = (dto.CriadoPorUsuarioId == usuarioId);
            }
            return dtos;
        }

        public async Task<List<ParoquiaResumoDto>> BuscarParoquiasAsync(string termo, int usuarioId)
        {
            if (string.IsNullOrWhiteSpace(termo))
                return new List<ParoquiaResumoDto>();

            var paroquias = await _paroquiaRepository.BuscarPorTermoAsync(termo);
            var dtos = new List<ParoquiaResumoDto>();

            foreach (var p in paroquias)
            {
                var dto = _mapper.Map<ParoquiaResumoDto>(p);
                var vinculo = await _paroquiaRepository.ObterVinculoUsuarioAsync(usuarioId, p.Id);
                if (vinculo != null)
                {
                    dto.JaParticipa = !vinculo.BloqueadoParaCodigo && vinculo.TentativasCodigoErrado == 0;
                    dto.BloqueadoCodigo = vinculo.BloqueadoParaCodigo;
                    dto.TentativasRestantes = Math.Max(0, 3 - vinculo.TentativasCodigoErrado);
                }
                else
                {
                    dto.JaParticipa = false;
                    dto.BloqueadoCodigo = false;
                    dto.TentativasRestantes = 3;
                }

                var solicitacaoPendente = await _paroquiaRepository.ObterSolicitacaoPendenteAsync(usuarioId, p.Id);
                dto.SolicitacaoPendente = solicitacaoPendente != null;

                dtos.Add(dto);
            }

            return dtos;
        }

        public async Task<List<ParoquiaDto>> ObterModelosAsync()
        {
            var paroquias = await _paroquiaRepository.ObterTodasAsync();
            return _mapper.Map<List<ParoquiaDto>>(paroquias.Where(p => p.Ativa));
        }

        public async Task<ParoquiaDto?> ObterPorIdAsync(int id, int? usuarioId = null)
        {
            var paroquia = await _paroquiaRepository.ObterPorIdAsync(id);
            if (paroquia == null) return null;

            var dto = _mapper.Map<ParoquiaDto>(paroquia);
            if (usuarioId.HasValue)
            {
                dto.EhCriador = (dto.CriadoPorUsuarioId == usuarioId.Value);
                var vinculo = await _paroquiaRepository.ObterVinculoUsuarioAsync(usuarioId.Value, id);
                if (vinculo != null)
                {
                    dto.BloqueadoParaCodigo = vinculo.BloqueadoParaCodigo;
                    dto.TentativasRestantes = Math.Max(0, 3 - vinculo.TentativasCodigoErrado);
                }
            }
            return dto;
        }

        public async Task<ParoquiaDto> CriarAsync(ParoquiaCriarDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new DomainException("Nome da paróquia é obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.Cidade))
                throw new DomainException("Cidade da paróquia é obrigatória.");
            if (string.IsNullOrWhiteSpace(dto.Estado))
                throw new DomainException("Estado da paróquia é obrigatório.");

            var codigoAcesso = await GerarCodigoAcessoUnicoAsync(dto.Nome);

            var novaParoquia = new Paroquia(
                dto.Nome.Trim(),
                dto.Cidade.Trim(),
                dto.Estado.Trim(),
                dto.Bairro?.Trim(),
                dto.Endereco?.Trim(),
                codigoAcesso,
                dto.CriadoPorUsuarioId
            );

            await _paroquiaRepository.AdicionarAsync(novaParoquia);

            // Vincula o criador como membro da paróquia
            var vinculoCriador = new UsuarioParoquia(dto.CriadoPorUsuarioId, novaParoquia.Id);
            await _paroquiaRepository.AdicionarVinculoUsuarioAsync(vinculoCriador);

            // Clonagem de Funções Litúrgicas
            List<Funcao> funcoesBase;
            if (dto.ModeloBaseParoquiaId.HasValue && dto.ModeloBaseParoquiaId.Value > 0)
            {
                funcoesBase = await _funcaoRepository.ObterTodasAsync(dto.ModeloBaseParoquiaId.Value);
                if (funcoesBase == null || !funcoesBase.Any())
                {
                    funcoesBase = await _funcaoRepository.ObterModelosBaseAsync();
                }
            }
            else
            {
                funcoesBase = await _funcaoRepository.ObterModelosBaseAsync();
            }

            foreach (var template in funcoesBase)
            {
                var novaFuncao = new Funcao(template.Nome)
                {
                    Abreviacao = template.Abreviacao,
                    EhMultipla = template.EhMultipla,
                    Cargo = template.Cargo,
                    NivelMinimo = template.NivelMinimo,
                    Obrigatoria = template.Obrigatoria,
                    ParoquiaId = novaParoquia.Id
                };
                await _funcaoRepository.AdicionarAsync(novaFuncao);
            }

            var paroquiaDto = _mapper.Map<ParoquiaDto>(novaParoquia);
            paroquiaDto.EhCriador = true;
            return paroquiaDto;
        }

        public async Task<(bool Sucesso, string Mensagem, ParoquiaDto? Paroquia)> EntrarPorCodigoAsync(EntrarCodigoDto dto)
        {
            var paroquia = await _paroquiaRepository.ObterPorIdAsync(dto.ParoquiaId);
            if (paroquia == null)
                return (false, "Paróquia não encontrada.", null);

            var vinculo = await _paroquiaRepository.ObterVinculoUsuarioAsync(dto.UsuarioId, dto.ParoquiaId);

            if (vinculo != null && vinculo.BloqueadoParaCodigo)
            {
                return (false, "Você errou o código 3 vezes e seu acesso por código foi bloqueado nesta paróquia. Solicite acesso ao coordenador.", null);
            }

            var codigoInformado = dto.CodigoAcesso?.Trim();
            var codigoCorreto = paroquia.CodigoAcesso.Trim();

            if (string.Equals(codigoInformado, codigoCorreto, StringComparison.OrdinalIgnoreCase))
            {
                if (vinculo != null)
                {
                    vinculo.ResetarTentativas();
                    await _paroquiaRepository.AtualizarVinculoUsuarioAsync(vinculo);
                }
                else
                {
                    vinculo = new UsuarioParoquia(dto.UsuarioId, dto.ParoquiaId);
                    await _paroquiaRepository.AdicionarVinculoUsuarioAsync(vinculo);
                }

                var paroquiaDto = _mapper.Map<ParoquiaDto>(paroquia);
                return (true, "Acesso concedido com sucesso!", paroquiaDto);
            }
            else
            {
                if (vinculo == null)
                {
                    vinculo = new UsuarioParoquia(dto.UsuarioId, dto.ParoquiaId);
                    vinculo.RegistrarTentativaInvalida();
                    await _paroquiaRepository.AdicionarVinculoUsuarioAsync(vinculo);
                }
                else
                {
                    vinculo.RegistrarTentativaInvalida();
                    await _paroquiaRepository.AtualizarVinculoUsuarioAsync(vinculo);
                }

                if (vinculo.BloqueadoParaCodigo)
                {
                    return (false, "Código incorreto. Você errou 3 vezes e seu acesso por código a esta paróquia foi bloqueado. Solicite acesso ao coordenador.", null);
                }
                else
                {
                    var restantes = 3 - vinculo.TentativasCodigoErrado;
                    return (false, $"Código incorreto. Você tem mais {restantes} tentativa(s).", null);
                }
            }
        }

        public async Task<(bool Sucesso, string Mensagem)> SolicitarAcessoAsync(SolicitarAcessoParoquiaDto dto)
        {
            var paroquia = await _paroquiaRepository.ObterPorIdAsync(dto.ParoquiaId);
            if (paroquia == null)
                return (false, "Paróquia não encontrada.");

            var vinculo = await _paroquiaRepository.ObterVinculoUsuarioAsync(dto.UsuarioId, dto.ParoquiaId);
            if (vinculo != null && !vinculo.BloqueadoParaCodigo && vinculo.TentativasCodigoErrado == 0)
            {
                return (false, "Você já participa desta paróquia.");
            }

            var pendente = await _paroquiaRepository.ObterSolicitacaoPendenteAsync(dto.UsuarioId, dto.ParoquiaId);
            if (pendente != null)
            {
                return (false, "Você já possui uma solicitação pendente para esta paróquia.");
            }

            var novaSolicitacao = new SolicitacaoAcessoParoquia(dto.UsuarioId, dto.ParoquiaId, dto.Mensagem);
            await _paroquiaRepository.AdicionarSolicitacaoAsync(novaSolicitacao);

            return (true, "Solicitação enviada com sucesso! Aguarde a aprovação do coordenador.");
        }

        public async Task<List<SolicitacaoAcessoParoquiaDto>> ObterSolicitacoesAsync(int paroquiaId)
        {
            var solicitacoes = await _paroquiaRepository.ObterSolicitacoesPorParoquiaIdAsync(paroquiaId);
            return _mapper.Map<List<SolicitacaoAcessoParoquiaDto>>(solicitacoes);
        }

        public async Task<(bool Sucesso, string Mensagem)> ResponderSolicitacaoAsync(int solicitacaoId, int respondidoPorUsuarioId, bool aprovar)
        {
            var solicitacao = await _paroquiaRepository.ObterSolicitacaoPorIdAsync(solicitacaoId);
            if (solicitacao == null)
                return (false, "Solicitação não encontrada.");

            if (solicitacao.Status != 1)
                return (false, "Esta solicitação já foi respondida anteriormente.");

            if (aprovar)
            {
                solicitacao.Aprovar(respondidoPorUsuarioId);
                var vinculo = await _paroquiaRepository.ObterVinculoUsuarioAsync(solicitacao.UsuarioId, solicitacao.ParoquiaId);
                if (vinculo != null)
                {
                    vinculo.ResetarTentativas();
                    await _paroquiaRepository.AtualizarVinculoUsuarioAsync(vinculo);
                }
                else
                {
                    vinculo = new UsuarioParoquia(solicitacao.UsuarioId, solicitacao.ParoquiaId);
                    await _paroquiaRepository.AdicionarVinculoUsuarioAsync(vinculo);
                }
            }
            else
            {
                solicitacao.Rejeitar(respondidoPorUsuarioId);
            }

            await _paroquiaRepository.AtualizarSolicitacaoAsync(solicitacao);
            return (true, aprovar ? "Solicitação aprovada com sucesso!" : "Solicitação rejeitada com sucesso!");
        }

        public async Task<(bool Sucesso, string Mensagem)> ReivindicarAsync(ReivindicarParoquiaDto dto)
        {
            var paroquia = await _paroquiaRepository.ObterPorIdAsync(dto.ParoquiaId);
            if (paroquia == null)
                return (false, "Paróquia não encontrada.");

            if (string.IsNullOrWhiteSpace(dto.Justificativa))
                return (false, "Justificativa é obrigatória.");

            if (string.IsNullOrWhiteSpace(dto.TelefoneContato))
                return (false, "Telefone de contato é obrigatório.");

            var reivindicacao = new ReivindicacaoParoquia(dto.UsuarioId, dto.ParoquiaId, dto.Justificativa.Trim(), dto.TelefoneContato.Trim());
            await _paroquiaRepository.AdicionarReivindicacaoAsync(reivindicacao);

            return (true, "Reivindicação enviada com sucesso! O suporte/administrador analisará sua solicitação.");
        }

        private async Task<string> GerarCodigoAcessoUnicoAsync(string nomeParoquia)
        {
            // Gera um prefixo limpo com até 4 letras do nome da paróquia
            var slug = Regex.Replace(nomeParoquia.ToUpper(), @"[^A-Z]", "");
            var prefixo = slug.Length >= 4 ? slug.Substring(0, 4) : slug.PadRight(4, 'P');

            for (int i = 0; i < 20; i++)
            {
                var sufixo = RandomNumberGenerator.GetInt32(1000, 9999);
                var codigo = $"{prefixo}-{sufixo}";
                var existente = await _paroquiaRepository.ObterPorCodigoAsync(codigo);
                if (existente == null)
                    return codigo;
            }

            return $"{prefixo}-{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}";
        }
    }
}
