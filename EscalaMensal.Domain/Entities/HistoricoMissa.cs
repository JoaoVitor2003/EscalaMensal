using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EscalaMensal.Domain.Entities
{
    public class HistoricoMissa
    {
        public int Id { get; private set; }
        public int MissaOriginalId { get; private set; }
        public int HistoricoEscalaId { get; private set; }
        
        [JsonIgnore]
        public HistoricoEscala? HistoricoEscala { get; private set; }

        public DateOnly Dia { get; private set; }
        public TimeOnly Horario { get; private set; }

        public ICollection<HistoricoItemMissa> HistoricoItensMissa { get; private set; } = new List<HistoricoItemMissa>();

        public HistoricoMissa(int missaOriginalId, int historicoEscalaId, DateOnly dia, TimeOnly horario)
        {
            MissaOriginalId = missaOriginalId;
            HistoricoEscalaId = historicoEscalaId;
            Dia = dia;
            Horario = horario;
        }

        private HistoricoMissa() { }
    }
}
