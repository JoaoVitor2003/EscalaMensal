using EscalaMensal.Application.DTOs.Funcao;
using EscalaMensal.Application.DTOs.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Application.DTOs.HistoricoEscala
{
    public class HistoricoEscalaAdicionarDto
    {
        public DateTime Data { get; set; }
        public int UsuarioId { get; set; }
        public int FuncaoId { get; set; }
    }
}
