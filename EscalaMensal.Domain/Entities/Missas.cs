using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Entities
{
    public class Missas
    {
        public int Id { get; private set; }
        public DateOnly Dia { get; private set; }
        public TimeOnly Horario { get; private set; }
        public int EscalaId { get; private set; }
        [JsonIgnore]
        public Escala? Escala { get; private set; }

        public ICollection<ItemMissa> ItensMissa { get; private set; } = new List<ItemMissa>();

        public Missas(DateOnly dia, TimeOnly horario, int escalaId)
        {
            Dia = dia;
            Horario = horario;
            EscalaId = escalaId;
        }
    }
}
