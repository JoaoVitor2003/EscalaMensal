using EscalaMensal.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Application.DTOs.CargoNivel
{
    public class CargoNivelDto
    {
        public int Id { get; set; }
        public CargoEnum Cargo { get; set; }
        public NivelEnum Nivel { get; set; }
        public int FuncaoId { get; set; }
    }
}
