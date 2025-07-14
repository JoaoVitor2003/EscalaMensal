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

        public ICollection<ItemEscala> Itens { get; private set; } = new List<ItemEscala>();

    }

}
