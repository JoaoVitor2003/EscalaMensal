using EscalaMensal.Application.DTOs.Missa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Application.DTOs.Escala
{
    public class EscalaDto
    {
        public int Id { get; set; }
        public DateOnly DataInicio { get; set; }
        public DateOnly DataFim { get; set; }
        public ICollection<MissasDto> Missas { get; set; } = new List<MissasDto>();
    }
}
