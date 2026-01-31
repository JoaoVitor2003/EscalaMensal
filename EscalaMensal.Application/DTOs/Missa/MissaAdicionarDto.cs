using EscalaMensal.Application.DTOs.Escala;
using EscalaMensal.Application.DTOs.ItemMissa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EscalaMensal.Application.DTOs.Missa
{
    public class MissaAdicionarDto
    {
        public DateOnly Dia { get; set; }
        public TimeOnly Horario { get; set; }
        public int EscalaId { get; set; }
    }
}
