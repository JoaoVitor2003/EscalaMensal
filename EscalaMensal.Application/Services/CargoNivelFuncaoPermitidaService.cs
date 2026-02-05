using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Enums;
using EscalaMensal.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class CargoNivelFuncaoPermitidaService : ICargoNivelFuncaoPermitidaService
    {
        private readonly ICargoNivelFuncaoPermitidaRepository _repository;

        public CargoNivelFuncaoPermitidaService(ICargoNivelFuncaoPermitidaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Funcao>> ObterFuncoesPermitidasAsync(CargoEnum cargo, NivelEnum nivel)
        {
            return await _repository.ObterFuncoesPermitidasAsync(cargo, nivel);
        }

        public async Task<List<CargoNivelFuncaoPermitida>> ObterTodasAsync()
        {
            return await _repository.ObterTodasAsync();
        }
    }
}
