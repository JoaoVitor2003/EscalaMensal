using EscalaMensal.Application.DTOs.Funcao;
using System.Collections.Generic;
using System;

namespace EscalaMensal.Application.DTOs.MissaPadrao
{
    public class MissaPadraoDto
    {
        public int Id { get; set; }
        public DayOfWeek DiaSemana { get; set; }
        public TimeOnly Horario { get; set; }
        public bool CriarFuncoesPadrao { get; set; }
        public List<FuncaoDto> Funcoes { get; set; } = new();
    }
}
