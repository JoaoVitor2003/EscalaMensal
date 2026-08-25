using System;
using System.Collections.Generic;

namespace EscalaMensal.Application.DTOs.MissaPadrao
{
    public class MissaPadraoAdicionarDto
    {
        public DayOfWeek DiaSemana { get; set; }
        public TimeOnly Horario { get; set; }
        public bool CriarFuncoesPadrao { get; set; } = true;
        public List<int> FuncoesIds { get; set; } = new();
    }
}
