using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Entities
{
    public class Escala
    {
        public int Id { get; private set; }
        public DateOnly DataInicio { get; private set; }
        public DateOnly DataFim { get; private set; }
        public ICollection<Missas> Missas { get; private set; } = new List<Missas>();
        public Escala(DateOnly inicio, DateOnly fim)
        {
            DataInicio = inicio;
            DataFim = fim;
        }

        public Missas AdicionarMissa(DateOnly dia, TimeOnly horario)
        {
            var missa = new Missas(dia, horario, this.Id);
            Missas.Add(missa);
            return missa;
        }
    }

}
