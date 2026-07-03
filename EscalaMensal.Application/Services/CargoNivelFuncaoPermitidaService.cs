using EscalaMensal.Domain.Entities;
using EscalaMensal.Domain.Enums;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscalaMensal.Application.Services
{
    public class CargoNivelFuncaoPermitidaService : ICargoNivelFuncaoPermitidaService
    {
        private readonly ICargoNivelFuncaoPermitidaRepository _repository;
        private readonly IConfiguracaoRepository _configuracaoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IFuncaoRepository _funcaoRepository;

        private static List<CargoNivelFuncaoPermitida> _backupPermissoesDeletadas = new();
        private static int _nivelRemovidoBackup = 0;

        public CargoNivelFuncaoPermitidaService(
            ICargoNivelFuncaoPermitidaRepository repository,
            IConfiguracaoRepository configuracaoRepository,
            IUsuarioRepository usuarioRepository,
            IFuncaoRepository funcaoRepository)
        {
            _repository = repository;
            _configuracaoRepository = configuracaoRepository;
            _usuarioRepository = usuarioRepository;
            _funcaoRepository = funcaoRepository;
        }

        public async Task<List<Funcao>> ObterFuncoesPermitidasAsync(CargoEnum cargo, NivelEnum nivel)
        {
            return await _repository.ObterFuncoesPermitidasAsync(cargo, nivel);
        }

        public async Task<List<CargoNivelFuncaoPermitida>> ObterTodasAsync()
        {
            return await _repository.ObterTodasAsync();
        }

        public async Task AdicionarAsync(CargoEnum cargo, NivelEnum nivel, int funcaoId)
        {
            var cargoNivel = new CargoNivelFuncaoPermitida(cargo, nivel, funcaoId);
            await _repository.AdicionarAsync(cargoNivel);
        }

        public async Task RemoverAsync(int id)
        {
            await _repository.RemoverAsync(id);
        }

        public async Task<int> ObterMaxNivelAsync()
        {
            return await _configuracaoRepository.ObterMaxNivelAsync();
        }

        public async Task<int> IncrementarNivelAsync()
        {
            var maxNivel = await _configuracaoRepository.ObterMaxNivelAsync();
            if (maxNivel < 10)
            {
                maxNivel++;
                await _configuracaoRepository.SalvarMaxNivelAsync(maxNivel);
            }
            return maxNivel;
        }

        public async Task RemoverNivelMaisAltoAsync()
        {
            var maxNivel = await _configuracaoRepository.ObterMaxNivelAsync();
            if (maxNivel <= 3)
            {
                throw new DomainException("Não é possível remover níveis abaixo do nível 3.");
            }

            var users = await _usuarioRepository.ObterTodosAsync();
            if (users != null && users.Any(u => (int)u.Nivel == maxNivel))
            {
                throw new DomainException($"Não é possível remover o nível {maxNivel} pois existem usuários associados a ele.");
            }

            var funcs = await _funcaoRepository.ObterTodasAsync();
            if (funcs != null && funcs.Any(f => (int)f.NivelMinimo == maxNivel))
            {
                throw new DomainException($"Não é possível remover o nível {maxNivel} pois existem funções configuradas com este nível mínimo.");
            }

            var todas = await _repository.ObterTodasAsync();
            var deletadas = todas.Where(p => (int)p.Nivel == maxNivel).ToList();

            _backupPermissoesDeletadas = deletadas;
            _nivelRemovidoBackup = maxNivel;

            foreach (var p in deletadas)
            {
                await _repository.RemoverAsync(p.Id);
            }

            maxNivel--;
            await _configuracaoRepository.SalvarMaxNivelAsync(maxNivel);
        }

        public async Task DesfazerRemocaoNivelAsync()
        {
            if (_nivelRemovidoBackup == 0)
            {
                throw new DomainException("Não há nenhuma remoção de nível para desfazer.");
            }

            await _configuracaoRepository.SalvarMaxNivelAsync(_nivelRemovidoBackup);

            if (_backupPermissoesDeletadas.Any())
            {
                foreach (var p in _backupPermissoesDeletadas)
                {
                    var novo = new CargoNivelFuncaoPermitida(p.Cargo, p.Nivel, p.FuncaoId);
                    await _repository.AdicionarAsync(novo);
                }
            }

            _backupPermissoesDeletadas.Clear();
            _nivelRemovidoBackup = 0;
        }
    }
}
