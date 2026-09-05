using AutoMapper;
using EscalaMensal.Application.DTOs.HistoricoEscala;
using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Domain.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EscalaMensal.Application.Services
{
    public class HistoricoEscalaService : IHistoricoEscalaService
    {
        private readonly IHistoricoEscalaRepository _historicoEscalaRepository;
        private readonly IEscalaRepository _escalaRepository;
        private readonly IMapper _mapper;

        public HistoricoEscalaService(IHistoricoEscalaRepository historicoEscalaRepository, IEscalaRepository escalaRepository, IMapper mapper)
        {
            _historicoEscalaRepository = historicoEscalaRepository;
            _escalaRepository = escalaRepository;
            _mapper = mapper;
        }

        public async Task<List<HistoricoEscalaDto>> ObterPorMesAnoAsync(int mes, int ano)
        {
            var historicos = await _historicoEscalaRepository.ObterPorMesAnoAsync(mes, ano);
            var dtos = _mapper.Map<List<HistoricoEscalaDto>>(historicos);
            return dtos;
        }

        public async Task<List<HistoricoEscalaDto>> ObterPorMembroIdAsync(int membroId)
        {
            var historicos = await _historicoEscalaRepository.ObterPorMembroIdAsync(membroId);
            var dtos = _mapper.Map<List<HistoricoEscalaDto>>(historicos);
            return dtos;
        }

        public async Task AdicionarAsync(HistoricoEscala historico)
        {
            await _historicoEscalaRepository.AdicionarAsync(historico);
        }

        public async Task FinalizarEscalaAsync(int escalaId)
        {
            var escala = await _escalaRepository.ObterPorIdAsync(escalaId);
            if (escala == null)
            {
                throw new DomainException("Escala não encontrada.");
            }

            var historicosExistentes = await _historicoEscalaRepository.ObterTodosPorEscalaOriginalIdAsync(escalaId);
            if (historicosExistentes.Count > 0)
            {
                // Ordena por DataFinalizacao decrescente para pegar o histórico mais recente gravado
                var historicoAtual = historicosExistentes[0]; 
                
                if (EscalaIgualAoHistorico(escala, historicoAtual))
                {
                    throw new DomainException("Esta escala já está finalizada e não possui alterações desde o último salvamento.");
                }
            }

            foreach (var hist in historicosExistentes)
            {
                await _historicoEscalaRepository.RemoverAsync(hist.Id);
            }

            var historicoEscala = new HistoricoEscala(
                escala.Id,
                escala.DataInicio,
                escala.DataFim,
                escala.LimitePermitido,
                DateTime.Now
            );

            foreach (var missa in escala.Missas)
            {
                var historicoMissa = new HistoricoMissa(
                    missa.Id,
                    0,
                    missa.Dia,
                    missa.Horario
                );

                foreach (var item in missa.ItensMissa)
                {
                    var historicoItem = new HistoricoItemMissa(
                        item.Id,
                        0,
                        item.FuncaoId,
                        item.Funcao?.Nome ?? "N/A",
                        item.Funcao?.Abreviacao ?? "N/A",
                        item.MembroId,
                        item.Membro?.Nome,
                        item.Ordem
                    );

                    historicoMissa.HistoricoItensMissa.Add(historicoItem);
                }

                historicoEscala.HistoricoMissas.Add(historicoMissa);
            }

            await _historicoEscalaRepository.AdicionarAsync(historicoEscala);
        }

        private bool EscalaIgualAoHistorico(Escala escala, HistoricoEscala historico)
        {
            if (escala.DataInicio != historico.DataInicio ||
                escala.DataFim != historico.DataFim ||
                escala.LimitePermitido != historico.LimitePermitido)
            {
                return false;
            }

            if (escala.Missas.Count != historico.HistoricoMissas.Count)
            {
                return false;
            }

            foreach (var missa in escala.Missas)
            {
                var histMissa = System.Linq.Enumerable.FirstOrDefault(historico.HistoricoMissas, m => m.MissaOriginalId == missa.Id);
                if (histMissa == null)
                {
                    return false;
                }

                if (missa.Dia != histMissa.Dia || missa.Horario != histMissa.Horario)
                {
                    return false;
                }

                if (missa.ItensMissa.Count != histMissa.HistoricoItensMissa.Count)
                {
                    return false;
                }

                foreach (var item in missa.ItensMissa)
                {
                    var histItem = System.Linq.Enumerable.FirstOrDefault(histMissa.HistoricoItensMissa, i => i.ItemMissaOriginalId == item.Id);
                    if (histItem == null)
                    {
                        return false;
                    }

                    if (item.FuncaoId != histItem.FuncaoId ||
                        item.MembroId != histItem.MembroId ||
                        item.Ordem != histItem.Ordem)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

