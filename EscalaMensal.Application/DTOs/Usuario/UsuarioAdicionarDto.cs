using EscalaMensal.Application.DTOs.Restricao;
using EscalaMensal.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Application.DTOs.Usuario
{
    public class UsuarioAdicionarDto
    {
        public string Nome { get; set; }
        public bool Ativo { get; set; } = true;
        public CargoEnum Cargo { get; set; }
        public NivelEnum Nivel { get; set; }
        public TimeOnly? HoraPreferencial { get; set; }
        public bool DisponivelSabado { get; set; }
        public bool DisponivelQuarta { get; set; }
        public bool DisponivelQuinta { get; set; }
        public int? UsuarioVinculadoId { get; set; } = null;
    }
}
