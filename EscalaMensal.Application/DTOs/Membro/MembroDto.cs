using EscalaMensal.Application.DTOs.Restricao;
using EscalaMensal.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Application.DTOs.Membro
{
    public class MembroDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }

        public CargoEnum Cargo { get; set; }
        public NivelEnum Nivel { get; set; }
        public List<TimeOnly> HorasPreferenciais { get; set; } = new List<TimeOnly>();
        public List<DayOfWeek> DiasDisponiveis { get; set; } = new List<DayOfWeek>();
        public int? MembroVinculadoId { get; set; }
        public MembroDto? MembroVinculado { get; set; }
        public int? DiasEscalados { get; set; }
        public ICollection<RestricaoDto> Restricoes { get; set; } = new List<RestricaoDto>();
    }
}
