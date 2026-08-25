using AutoMapper;
using EscalaMensal.Application.DTOs.Escala;    
using EscalaMensal.Application.DTOs.ItemMissa;
using EscalaMensal.Application.DTOs.Missa;
using EscalaMensal.Domain.Entities;
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

        public EscalaService(IEscalaRepository escalaRepository, IMapper mapper, IMissaPadraoRepository missaPadraoRepository, IFuncaoRepository funcaoRepository)
        {
            _escalaRepository = escalaRepository;
            _mapper = mapper;
            _missaPadraoRepository = missaPadraoRepository;
            _funcaoRepository = funcaoRepository;
        }

        public async Task<EscalaDto?> ObterPorMesAnoAsync(int mes, int ano)
        {
            var escala = await _escalaRepository.ObterPorMesAnoAsync(mes, ano);
            var dto = _mapper.Map<EscalaDto>(escala);
            return dto;
        }

        public async Task<List<EscalaDto>> ObterTodasAsync()
        {
            var escalas = await _escalaRepository.ObterTodasAsync();
            var dto = _mapper.Map<List<EscalaDto>>(escalas);

            return dto;
        }

        public async Task AdicionarAsync(EscalaAdicionarDto escala)
        {
            var novaEscalaEntity = _mapper.Map<Escala>(escala);

            if (escala.CriarMissasPadrao)
            {
                var missasPadrao = await _missaPadraoRepository.ObterTodasAsync();
                var funcoesObrigatorias = await _funcaoRepository.ObterObrigatoriasAsync();

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
                                
                                cellNome.Value = item?.Usuario?.Nome ?? "-";
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
