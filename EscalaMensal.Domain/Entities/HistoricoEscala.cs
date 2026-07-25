using System;
using System.Collections.Generic;

namespace EscalaMensal.Domain.Entities
{
    public class HistoricoEscala
    {
        public int Id { get; private set; }
        public int EscalaOriginalId { get; private set; }
        public DateOnly DataInicio { get; private set; }
        public DateOnly DataFim { get; private set; }
        public int LimitePermitido { get; private set; }
        public DateTime DataFinalizacao { get; private set; }

        public ICollection<HistoricoMissa> HistoricoMissas { get; private set; } = new List<HistoricoMissa>();

        public HistoricoEscala(int escalaOriginalId, DateOnly dataInicio, DateOnly dataFim, int limitePermitido, DateTime dataFinalizacao)
        {
            EscalaOriginalId = escalaOriginalId;
            DataInicio = dataInicio;
            DataFim = dataFim;
            LimitePermitido = limitePermitido;
            DataFinalizacao = dataFinalizacao;
        }

        private HistoricoEscala() { }
    }
}
