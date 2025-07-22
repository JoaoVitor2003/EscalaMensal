﻿using EscalaMensal.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IHistoricoEscalaService
    {
        Task<List<HistoricoEscala>> ObterPorMesAnoAsync(int mes, int ano);
        Task<List<HistoricoEscala>> ObterPorUsuarioIdAsync(int usuarioId);
        Task AdicionarAsync(HistoricoEscala historico);
    }
}
