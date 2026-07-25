using System;
using System.Collections.Generic;

namespace EscalaMensal.Application.DTOs.HistoricoEscala
{
    public class HistoricoMissaDto
    {
        public int Id { get; set; }
        public int MissaOriginalId { get; set; }
        public int HistoricoEscalaId { get; set; }
        public DateOnly Dia { get; set; }
        public TimeOnly Horario { get; set; }
        public List<HistoricoItemMissaDto> HistoricoItensMissa { get; set; } = new List<HistoricoItemMissaDto>();
    }
}
