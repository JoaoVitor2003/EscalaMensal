using AutoMapper;
using EscalaMensal.Application.DTOs.Escala;    
using EscalaMensal.Application.DTOs.ItemMissa;
using EscalaMensal.Application.DTOs.Missa;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Enums;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Domain.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Globalization;
using System.IO;
using ClosedXML.Excel;

namespace EscalaMensal.Application.Services    
{
    public class EscalaService : IEscalaService
    {
        private readonly IEscalaRepository _escalaRepository;
        private readonly IMapper _mapper;
        private readonly IMissaPadraoRepository _missaPadraoRepository;
        private readonly IFuncaoRepository _funcaoRepository;
        private readonly IMembroRepository _membroRepository;
        private readonly ICargoNivelFuncaoPermitidaRepository _cargoNivelRepository;
        private readonly IRestricaoRepository _restricaoRepository;

        public EscalaService(
            IEscalaRepository escalaRepository, 
            IMapper mapper, 
            IMissaPadraoRepository missaPadraoRepository, 
            IFuncaoRepository funcaoRepository,
            IMembroRepository membroRepository,
            ICargoNivelFuncaoPermitidaRepository cargoNivelRepository,
            IRestricaoRepository restricaoRepository)
        {
            _escalaRepository = escalaRepository;
            _mapper = mapper;
            _missaPadraoRepository = missaPadraoRepository;
            _funcaoRepository = funcaoRepository;
            _membroRepository = membroRepository;
            _cargoNivelRepository = cargoNivelRepository;
            _restricaoRepository = restricaoRepository;
        }

        public async Task<EscalaDto?> ObterPorMesAnoAsync(int mes, int ano, int? usuarioId = null, int? perfil = null, int? paroquiaId = null)
        {
            var escala = await _escalaRepository.ObterPorMesAnoAsync(mes, ano, usuarioId, perfil, paroquiaId);
            var dto = _mapper.Map<EscalaDto>(escala);
            return dto;
        }

        public async Task<List<EscalaDto>> ObterTodasAsync(int? usuarioId = null, int? perfil = null, int? paroquiaId = null)
        {
            var escalas = await _escalaRepository.ObterTodasAsync(usuarioId, perfil, paroquiaId);
            var dto = _mapper.Map<List<EscalaDto>>(escalas);

            return dto;
        }

        public async Task AdicionarAsync(EscalaAdicionarDto escala)
        {
            var novaEscalaEntity = _mapper.Map<Escala>(escala);
            if (escala.CriadoPorUsuarioId.HasValue)
            {
                novaEscalaEntity.DefinirCriador(escala.CriadoPorUsuarioId.Value);
            }
            if (escala.ParoquiaId.HasValue)
            {
                novaEscalaEntity.DefinirParoquia(escala.ParoquiaId.Value);
            }

            if (escala.CriarMissasPadrao)
            {
                var missasPadrao = await _missaPadraoRepository.ObterTodasAsync(escala.ParoquiaId);
                var funcoesObrigatorias = await _funcaoRepository.ObterObrigatoriasAsync(escala.ParoquiaId);

                var dataAtual = escala.DataInicio;
                while (dataAtual <= escala.DataFim)
                {
                    var diaSemana = dataAtual.DayOfWeek;
                    var templatesDoDia = missasPadrao.Where(m => m.DiaSemana == diaSemana).ToList();

                    foreach (var template in templatesDoDia)
                    {
                        var missa = novaEscalaEntity.AdicionarMissa(dataAtual, template.Horario);
                        if (template.CriarFuncoesPadrao)
                        {
                            foreach (var funcao in funcoesObrigatorias)
                            {
                                missa.ItensMissa.Add(new ItemMissa(missaId: 0, funcaoId: funcao.Id));
                            }
                        }
                        else
                        {
                            foreach (var funcao in template.Funcoes)
                            {
                                missa.ItensMissa.Add(new ItemMissa(missaId: 0, funcaoId: funcao.Id));
                            }
                        }
                    }

                    dataAtual = dataAtual.AddDays(1);
                }
            }

            await _escalaRepository.AdicionarAsync(novaEscalaEntity);
        }

        public async Task AtualizarAsync(EscalaAtualizarDto escalaDto)
        {
            var escala = await _escalaRepository.ObterPorIdAsync(escalaDto.Id);

            if (escala == null)
                throw new DomainException("Escala não encontrada.");

            _mapper.Map(escalaDto, escala);

            await _escalaRepository.AtualizarAsync(escala);
        }

        public async Task RemoverAsync(int id)
        {
            await _escalaRepository.RemoverAsync(id);
        }

        public async Task<EscalaDto?> ObterPorIdAsync(int id)
        {
            var escala = await _escalaRepository.ObterPorIdAsync(id);

            var dto = _mapper.Map<EscalaDto>(escala);
            return dto;
        }

        public async Task<GerarEscalaAleatoriaResultadoDto> GerarEscalaAleatoriaAsync(int escalaId, GerarEscalaAleatoriaRequestDto request)
        {
            // 1. Validar perfil Premium / Administrador
            if (request.Perfil.HasValue && 
                request.Perfil.Value != (int)PerfilUsuarioEnum.Administrador && 
                request.Perfil.Value != (int)PerfilUsuarioEnum.Premium)
            {
                return new GerarEscalaAleatoriaResultadoDto
                {
                    Sucesso = false,
                    Mensagem = "Apenas usuários com perfil Premium ou Administrador podem utilizar a geração automática de escala."
                };
            }

            // 2. Carregar escala
            var escala = await _escalaRepository.ObterPorIdAsync(escalaId);
            if (escala == null)
            {
                return new GerarEscalaAleatoriaResultadoDto { Sucesso = false, Mensagem = "Escala não encontrada." };
            }

            if (!escala.Missas.Any())
            {
                return new GerarEscalaAleatoriaResultadoDto { Sucesso = false, Mensagem = "A escala não possui celebrações/missas cadastradas." };
            }

            // 3. Obter membros ativos da paróquia
            var todosMembros = await _membroRepository.ObterTodosAsync(escala.ParoquiaId);
            var membrosAtivos = todosMembros.Where(m => m.Ativo).ToList();
            if (!membrosAtivos.Any())
            {
                return new GerarEscalaAleatoriaResultadoDto { Sucesso = false, Mensagem = "Não há coroinhas/acólitos ativos cadastrados nesta paróquia para alocação." };
            }

            // 4. Carregar restrições de data
            var mesesAnos = escala.Missas
                .Select(m => new { m.Dia.Month, m.Dia.Year })
                .Distinct()
                .ToList();

            var restricoes = new List<Restricao>();
            foreach (var ma in mesesAnos)
            {
                var rMes = await _restricaoRepository.ObterPorMesAnoAsync(ma.Month, ma.Year);
                if (rMes != null)
                {
                    restricoes.AddRange(rMes);
                }
            }

            var restricoesPorMembro = restricoes
                .GroupBy(r => r.MembroId)
                .ToDictionary(g => g.Key, g => g.Select(r => DateOnly.FromDateTime(r.Data)).ToHashSet());

            // 5. Carregar permissões de funções
            var permissoes = await _cargoNivelRepository.ObterTodasAsync();

            // 6. Contagem de participações e dias distintos escalados
            var contagemParticipacoes = membrosAtivos.ToDictionary(m => m.Id, m => 0);
            var diasEscaladosPorMembro = membrosAtivos.ToDictionary(m => m.Id, m => new HashSet<DateOnly>());

            if (request.ApenasVagasVazias)
            {
                foreach (var missa in escala.Missas)
                {
                    foreach (var item in missa.ItensMissa)
                    {
                        if (item.MembroId.HasValue && contagemParticipacoes.ContainsKey(item.MembroId.Value))
                        {
                            contagemParticipacoes[item.MembroId.Value]++;
                            diasEscaladosPorMembro[item.MembroId.Value].Add(missa.Dia);
                        }
                    }
                }
            }
            else
            {
                // Limpar todas as alocações anteriores
                foreach (var missa in escala.Missas)
                {
                    foreach (var item in missa.ItensMissa)
                    {
                        item.RemoverMembro();
                    }
                }
            }

            int totalAlocadosNestaRodada = 0;
            var rand = new Random();

            // 7. Ordenar as missas por data e horário
            var missasOrdenadas = escala.Missas.OrderBy(m => m.Dia).ThenBy(m => m.Horario).ToList();

            foreach (var missa in missasOrdenadas)
            {
                var membrosNaMissa = missa.ItensMissa
                    .Where(i => i.MembroId.HasValue)
                    .Select(i => i.MembroId.Value)
                    .ToHashSet();

                var membrosNoDia = escala.Missas
                    .Where(m => m.Dia == missa.Dia)
                    .SelectMany(m => m.ItensMissa)
                    .Where(i => i.MembroId.HasValue)
                    .Select(i => i.MembroId.Value)
                    .ToHashSet();

                // Ordena os itens colocando primeiro cerimoniários e funções mais específicas
                var itensParaPreencher = missa.ItensMissa
                    .Where(i => !request.ApenasVagasVazias || !i.MembroId.HasValue)
                    .OrderByDescending(i => i.Funcao?.Cargo == CargoEnum.Cerimoniario ? 1 : 0)
                    .ThenBy(i => i.Ordem)
                    .ToList();

                foreach (var item in itensParaPreencher)
                {
                    if (item.MembroId.HasValue && request.ApenasVagasVazias)
                        continue;

                    var funcao = item.Funcao;

                    // Candidatos válidos estritamente
                    var candidatos = membrosAtivos.Where(m =>
                    {
                        // Não pode repetir na mesma missa
                        if (membrosNaMissa.Contains(m.Id))
                            return false;

                        // Cargo e Nível
                        if (funcao != null)
                        {
                            if (funcao.Cargo == CargoEnum.Cerimoniario && (m.Cargo & CargoEnum.Cerimoniario) == 0)
                                return false;

                            if (permissoes.Any(p => p.FuncaoId == funcao.Id))
                            {
                                bool temPermissao = permissoes.Any(p =>
                                    p.FuncaoId == funcao.Id &&
                                    ((p.Cargo == m.Cargo && p.Nivel <= m.Nivel) || p.Cargo < m.Cargo));

                                if (!temPermissao)
                                    return false;
                            }
                        }

                        // Restrição de data
                        if (request.RespeitarRestricoes && restricoesPorMembro.TryGetValue(m.Id, out var datasRestritas))
                        {
                            if (datasRestritas.Contains(missa.Dia))
                                return false;
                        }

                        // Dias disponíveis da semana
                        if (m.DiasDisponiveis != null && m.DiasDisponiveis.Any())
                        {
                            if (!m.DiasDisponiveis.Contains(missa.Dia.DayOfWeek))
                                return false;
                        }

                        // Limite estrito de vezes e de dias permitidos na escala
                        if (request.RespeitarLimite && escala.LimitePermitido > 0)
                        {
                            // Não pode ultrapassar a quantidade máxima de participações permitidas
                            if (contagemParticipacoes[m.Id] >= escala.LimitePermitido)
                                return false;

                            // Não pode ultrapassar a quantidade máxima de dias distintos escalados
                            if (diasEscaladosPorMembro[m.Id].Count >= escala.LimitePermitido && !diasEscaladosPorMembro[m.Id].Contains(missa.Dia))
                                return false;
                        }

                        return true;
                    }).ToList();

                    // Se ninguém qualificado estiver abaixo do limite, a vaga permanece aberta (sem violar o teto)
                    if (!candidatos.Any())
                        continue;

                    // Priorização: Evitar mesmo dia (se configurado)
                    var pool = candidatos;
                    if (request.EvitarMesmoDia)
                    {
                        var semMissaNoDia = candidatos.Where(c => !membrosNoDia.Contains(c.Id)).ToList();
                        if (semMissaNoDia.Any())
                        {
                            pool = semMissaNoDia;
                        }
                    }

                    // Priorização: Menor contagem de participações para distribuição equilibrada
                    int menorParticipacao = pool.Min(c => contagemParticipacoes[c.Id]);
                    var empatados = pool.Where(c => contagemParticipacoes[c.Id] == menorParticipacao).ToList();

                    // Sorteio aleatório entre os melhores empatados
                    var escolhido = empatados[rand.Next(empatados.Count)];

                    item.AtribuirMembro(escolhido.Id);
                    contagemParticipacoes[escolhido.Id]++;
                    diasEscaladosPorMembro[escolhido.Id].Add(missa.Dia);
                    membrosNaMissa.Add(escolhido.Id);
                    membrosNoDia.Add(escolhido.Id);
                    totalAlocadosNestaRodada++;

                    // Alocação de Irmão / Membro Vinculado na mesma celebração
                    if (request.AlocarIrmaosJuntos && escolhido.MembroVinculadoId.HasValue && !membrosNaMissa.Contains(escolhido.MembroVinculadoId.Value))
                    {
                        var irmao = membrosAtivos.FirstOrDefault(m => m.Id == escolhido.MembroVinculadoId.Value);
                        if (irmao != null)
                        {
                            bool podeAlocarIrmao = true;

                            if (request.RespeitarLimite && escala.LimitePermitido > 0)
                            {
                                if (contagemParticipacoes[irmao.Id] >= escala.LimitePermitido)
                                    podeAlocarIrmao = false;

                                if (diasEscaladosPorMembro[irmao.Id].Count >= escala.LimitePermitido && !diasEscaladosPorMembro[irmao.Id].Contains(missa.Dia))
                                    podeAlocarIrmao = false;
                            }

                            if (podeAlocarIrmao)
                            {
                                var outraVaga = missa.ItensMissa.FirstOrDefault(i =>
                                    i.Id != item.Id &&
                                    (!request.ApenasVagasVazias || !i.MembroId.HasValue) &&
                                    !i.MembroId.HasValue);

                                if (outraVaga != null)
                                {
                                    bool irmaoAptoFuncao = true;
                                    if (outraVaga.Funcao != null)
                                    {
                                        if (outraVaga.Funcao.Cargo == CargoEnum.Cerimoniario && (irmao.Cargo & CargoEnum.Cerimoniario) == 0)
                                            irmaoAptoFuncao = false;

                                        if (permissoes.Any(p => p.FuncaoId == outraVaga.Funcao.Id))
                                        {
                                            irmaoAptoFuncao = permissoes.Any(p =>
                                                p.FuncaoId == outraVaga.Funcao.Id &&
                                                ((p.Cargo == irmao.Cargo && p.Nivel <= irmao.Nivel) || p.Cargo < irmao.Cargo));
                                        }
                                    }

                                    if (request.RespeitarRestricoes && restricoesPorMembro.TryGetValue(irmao.Id, out var rIrmao) && rIrmao.Contains(missa.Dia))
                                    {
                                        irmaoAptoFuncao = false;
                                    }

                                    if (irmao.DiasDisponiveis != null && irmao.DiasDisponiveis.Any() && !irmao.DiasDisponiveis.Contains(missa.Dia.DayOfWeek))
                                    {
                                        irmaoAptoFuncao = false;
                                    }

                                    if (irmaoAptoFuncao)
                                    {
                                        outraVaga.AtribuirMembro(irmao.Id);
                                        contagemParticipacoes[irmao.Id]++;
                                        diasEscaladosPorMembro[irmao.Id].Add(missa.Dia);
                                        membrosNaMissa.Add(irmao.Id);
                                        membrosNoDia.Add(irmao.Id);
                                        totalAlocadosNestaRodada++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            await _escalaRepository.AtualizarAsync(escala);

            var totalVagas = escala.Missas.SelectMany(m => m.ItensMissa).Count();
            var vagasRestantes = escala.Missas.SelectMany(m => m.ItensMissa).Count(i => !i.MembroId.HasValue);

            string mensagem;
            if (vagasRestantes == 0)
            {
                mensagem = $"Escala gerada com 100% de sucesso! Todas as {totalAlocadosNestaRodada} funções foram preenchidas respeitando o limite de {escala.LimitePermitido} vezes.";
            }
            else
            {
                mensagem = $"{totalAlocadosNestaRodada} funções foram preenchidas respeitando rigorosamente o limite de {escala.LimitePermitido} vezes/dias. {vagasRestantes} vagas permaneceram abertas para não exceder a cota máxima dos membros.";
            }

            return new GerarEscalaAleatoriaResultadoDto
            {
                Sucesso = true,
                TotalAlocados = totalAlocadosNestaRodada,
                TotalVagas = totalVagas,
                VagasRestantes = vagasRestantes,
                Mensagem = mensagem
            };
        }

        public byte[] GerarPlanilhaExcel(EscalaDto escala)
        {
            if (escala == null) return Array.Empty<byte>();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Escala de Missas");
                worksheet.ShowGridLines = true;
                
                var culture = new CultureInfo("pt-BR");

                worksheet.Cell(1, 1).Value = $"ESCALA DE MISSAS #{escala.Id}";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Font.FontSize = 14;

                worksheet.Cell(2, 1).Value = $"Período: {escala.DataInicio.ToString("dd/MM/yyyy", culture)} a {escala.DataFim.ToString("dd/MM/yyyy", culture)}";
                worksheet.Cell(2, 1).Style.Font.Italic = true;
                worksheet.Cell(3, 1).Value = $"Limite de vezes para servir: {escala.LimitePermitido}";
                worksheet.Cell(3, 1).Style.Font.Italic = true;

                var missasPorDia = escala.Missas
                    .Where(m => m != null)
                    .GroupBy(m => m.Dia)
                    .OrderBy(g => g.Key)
                    .ToList();

                var maxMissasNumDia = missasPorDia
                    .Select(g => g.Count())
                    .DefaultIfEmpty(0)
                    .Max();

                if (maxMissasNumDia == 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        workbook.SaveAs(ms);
                        return ms.ToArray();
                    }
                }

                int currentRow = 5;

                foreach (var grupoDia in missasPorDia)
                {
                    var data = grupoDia.Key;
                    var diaSemana = culture.TextInfo.ToTitleCase(data.ToString("dddd", culture));
                    var textoData = $"{data.ToString("dd/MM/yyyy", culture)} - {diaSemana}";

                    var missasDoDia = grupoDia.OrderBy(m => m.Horario).ToList();
                    int numMissasDia = missasDoDia.Count;
                    int colunasUsadasDia = numMissasDia * 2;

                    int colunasParaMesclar = Math.Max(2, colunasUsadasDia);
                    var rangeData = worksheet.Range(currentRow, 1, currentRow, colunasParaMesclar);
                    rangeData.Merge();
                    rangeData.Value = textoData;
                    rangeData.Style.Font.Bold = true;
                    rangeData.Style.Font.FontSize = 11;
                    rangeData.Style.Fill.BackgroundColor = XLColor.FromHtml("#D3D3D3"); // Cinza claro
                    rangeData.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rangeData.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    rangeData.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    
                    currentRow++;

                    for (int j = 0; j < numMissasDia; j++)
                    {
                        int startCol = j * 2 + 1;
                        int endCol = startCol + 1;
                        var rangeHorario = worksheet.Range(currentRow, startCol, currentRow, endCol);
                        rangeHorario.Merge();
                        rangeHorario.Value = missasDoDia[j].Horario.ToString("HH:mm", culture);
                        rangeHorario.Style.Font.Bold = true;
                        rangeHorario.Style.Fill.BackgroundColor = XLColor.FromHtml("#EAEAEA"); // Cinza muito claro
                        rangeHorario.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        rangeHorario.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }
                    
                    if (colunasParaMesclar > colunasUsadasDia)
                    {
                        worksheet.Range(currentRow, colunasUsadasDia + 1, currentRow, colunasParaMesclar).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                    currentRow++;

                    var maxItensDia = missasDoDia
                        .Select(m => m.ItensMissa?.Count ?? 0)
                        .DefaultIfEmpty(0)
                        .Max();

                    for (int i = 0; i < maxItensDia; i++)
                    {
                        for (int j = 0; j < numMissasDia; j++)
                        {
                            var missa = missasDoDia[j];
                            int colFuncao = j * 2 + 1;
                            int colNome = colFuncao + 1;

                            var cellFuncao = worksheet.Cell(currentRow, colFuncao);
                            var cellNome = worksheet.Cell(currentRow, colNome);

                            cellFuncao.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellNome.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            var itensMissaOrdenados = (missa.ItensMissa ?? new List<ItemMissaDto>())
                                .OrderBy(item => item.Ordem)
                                .ToList();

                            if (i < itensMissaOrdenados.Count)
                            {
                                var item = itensMissaOrdenados[i];
                                cellFuncao.Value = item?.Funcao?.Abreviacao ?? "";
                                cellFuncao.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                
                                cellNome.Value = item?.Membro?.Nome ?? "-";
                                cellNome.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            }
                            else
                            {
                                cellFuncao.Value = string.Empty;
                                cellNome.Value = string.Empty;
                            }
                        }
                        currentRow++;
                    }

                    currentRow++;
                }

                int totalColumns = maxMissasNumDia * 2;
                for (int col = 1; col <= totalColumns; col++)
                {
                    if (col % 2 != 0)
                    {
                        worksheet.Column(col).Width = 6;
                    }
                    else
                    {
                        worksheet.Column(col).AdjustToContents();
                    }
                }

                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return ms.ToArray();
                }
            }
        }
    }
}
