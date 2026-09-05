using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Entities
{
    public class Restricao
    {
        public int Id { get; private set; }
        public int MembroId { get; private set; }
        public Membro Membro { get; private set; }

        public DateTime Data { get; private set; }

        public Restricao(int membroId, DateTime data)
        {
            MembroId = membroId;
            Data = data.Date;
        }
    }

}
