using EscalaMensal.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Entities
{
    public class CargoNivelFuncaoPermitida
    {
        public int Id { get; private set; }

        public CargoEnum Cargo { get; private set; }
        public NivelEnum Nivel { get; private set; }

        public int FuncaoId { get; private set; }
        public Funcao Funcao { get; private set; }

        public CargoNivelFuncaoPermitida(CargoEnum cargo, NivelEnum nivel, int funcaoId)
        {
            Cargo = cargo;
            Nivel = nivel;
            FuncaoId = funcaoId;
        }
    }
}
