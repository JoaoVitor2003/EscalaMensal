using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface ICargoNivelFuncaoPermitidaRepository
    {
        Task<List<Funcao>> ObterFuncoesPermitidasAsync(CargoEnum cargo, NivelEnum nivel);
        Task<List<CargoNivelFuncaoPermitida>> ObterTodasAsync();
    }
}
