using EscalaMensal.Application.DTOs.Funcao;
using EscalaMensal.Application.DTOs.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Application.DTOs.HistoricoEscala
{
    public class HistoricoEscalaDto
    {
        public int Id { get; set; }

        public DateTime Data { get; set; }

        public int UsuarioId { get; set; }
        public UsuarioDto Usuario { get; set; }

        public int FuncaoId { get; set; }
        public FuncaoDto Funcao { get; set; }
    }
}
