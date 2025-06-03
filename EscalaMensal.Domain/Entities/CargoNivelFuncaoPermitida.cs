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

        public int CargoId { get; private set; }
        public Cargo Cargo { get; private set; }

        public int NivelId { get; private set; }
        public Nivel Nivel { get; private set; }

        public int FuncaoId { get; private set; }
        public Funcao Funcao { get; private set; }

        public CargoNivelFuncaoPermitida(int cargoId, int nivelId, int funcaoId)
        {
            CargoId = cargoId;
            NivelId = nivelId;
            FuncaoId = funcaoId;
        }
    }

}
