using System;

namespace EscalaMensal.Application.DTOs.MissaPadrao
{
    public class MissaPadraoDto
    {
        public int Id { get; set; }
        public DayOfWeek DiaSemana { get; set; }
        public TimeOnly Horario { get; set; }
    }
}
