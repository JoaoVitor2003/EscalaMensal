using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Application.DTOs.HistoricoEscala
{
    public class HistoricoEscalaAtualizarDto
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public int UsuarioId { get; set; }
        public int FuncaoId { get; set; }
    }
}
