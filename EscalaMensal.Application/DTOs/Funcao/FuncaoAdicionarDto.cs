using EscalaMensal.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Application.DTOs.Funcao
{
    public class FuncaoAdicionarDto
    {
        public string Nome { get; set; }
        public string Abreviacao { get; set; }

        public CargoEnum Cargo { get; set; }
        public NivelEnum NivelMinimo { get; set; }
        public bool Obrigatoria { get; set; }
    }
}
