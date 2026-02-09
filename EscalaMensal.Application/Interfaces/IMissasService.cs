using EscalaMensal.Application.DTOs.Missa;
using EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    public interface IMissasService
    {
        Task<MissasDto> ObterPorMissaIdAsync(int missaId);
        Task AdicionarAsync(MissaAdicionarDto missa);
        Task AtualizarAsync(MissaAtualizarDto missa);
        Task RemoverAsync(int id);
    }
}
