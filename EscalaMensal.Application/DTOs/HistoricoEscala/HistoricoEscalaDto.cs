using System;
using System.Collections.Generic;

namespace EscalaMensal.Application.DTOs.HistoricoEscala
{
    public class HistoricoEscalaDto
    {
        public int Id { get; set; }
        public int EscalaOriginalId { get; set; }
        public DateOnly DataInicio { get; set; }
        public DateOnly DataFim { get; set; }
        public int LimitePermitido { get; set; }
        public DateTime DataFinalizacao { get; set; }
        public List<HistoricoMissaDto> HistoricoMissas { get; set; } = new List<HistoricoMissaDto>();
    }
}
