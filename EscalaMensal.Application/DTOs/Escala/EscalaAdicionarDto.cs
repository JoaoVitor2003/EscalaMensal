using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Application.DTOs.Escala
{
    public class EscalaAdicionarDto
    {
        public DateOnly DataInicio { get; set; }
        public DateOnly DataFim { get; set; }
    }
}
