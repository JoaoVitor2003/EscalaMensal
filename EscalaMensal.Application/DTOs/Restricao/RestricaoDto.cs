using EscalaMensal.Application.DTOs.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Application.DTOs.Restricao
{
    public class RestricaoDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public UsuarioDto Usuario { get; set; }

        public DateTime Data { get; set; }
    }
}
