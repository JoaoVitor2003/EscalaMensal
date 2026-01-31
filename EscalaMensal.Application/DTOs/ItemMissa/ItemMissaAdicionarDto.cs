using EscalaMensal.Application.DTOs.Funcao;
using EscalaMensal.Application.DTOs.Missa;
using EscalaMensal.Application.DTOs.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EscalaMensal.Application.DTOs.ItemMissa
{
    public class ItemMissaAdicionarDto
    {
        public int MissaId { get; set; }
        public int FuncaoId { get; set; }
        public int? UsuarioId { get; set; }
    }
}
