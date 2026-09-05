using EscalaMensal.Application.DTOs.Membro;
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
        public int MembroId { get; set; }
        public MembroDto Membro { get; set; }

        public DateTime Data { get; set; }
    }
}
