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
        public int UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; }

        public DateTime Data { get; private set; }

        public Restricao(int usuarioId, DateTime data)
        {
            UsuarioId = usuarioId;
            Data = data.Date;
        }
    }

}
