using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IFuncaoRepository
    {
        Task<List<Funcao>> ObterTodasAsync(int? paroquiaId = null);
        Task<List<Funcao>> ObterObrigatoriasAsync(int? paroquiaId = null);
        Task<List<Funcao>> ObterModelosBaseAsync();
        Task<Funcao?> ObterPorIdAsync(int id);
        Task<List<Funcao>> ObterPorIdsAsync(List<int> ids);
        Task AdicionarAsync(Funcao funcao);
        Task AtualizarAsync(Funcao funcao);
        Task RemoverAsync(int id);
    }
}
