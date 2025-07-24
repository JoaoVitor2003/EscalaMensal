﻿using EscalaMensal.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IEscalaService
    {
        Task<Escala?> ObterPorMesAnoAsync(int mes, int ano);
        Task<List<Escala>> ObterTodasAsync();
        Task AdicionarAsync(Escala escala);
        Task AtualizarAsync(Escala escala);
        Task RemoverAsync(int id);
    }
}
