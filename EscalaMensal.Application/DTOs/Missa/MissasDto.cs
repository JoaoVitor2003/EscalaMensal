using EscalaMensal.Application.DTOs.Escala;
using EscalaMensal.Application.DTOs.ItemMissa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EscalaMensal.Application.DTOs.Missa
{
    public class MissasDto
    {
        public int Id { get; set; }
        public DateOnly Dia { get; set; }
        public TimeOnly Horario { get; set; }
        public int EscalaId { get; set; }
        [JsonIgnore]
        public EscalaDto? Escala { get; set; }

        public ICollection<ItemMissaDto> ItensMissa { get; set; } = new List<ItemMissaDto>();
    }
}
