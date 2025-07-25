using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface ICargoNivelFuncaoPermitidaService
    {
        Task<List<Funcao>> ObterFuncoesPermitidasAsync(CargoEnum cargo, NivelEnum nivel);
        Task<List<CargoNivelFuncaoPermitida>> ObterTodasAsync();
    }
}
