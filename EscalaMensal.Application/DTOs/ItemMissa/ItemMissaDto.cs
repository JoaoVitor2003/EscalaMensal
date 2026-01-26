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
    public class ItemMissaDto
    {
        public int Id { get; set; }
        public int MissaId { get; set; }
        [JsonIgnore]
        public MissasDto? Missa { get; set; }
        public int FuncaoId { get; set; }
        public FuncaoDto? Funcao { get; set; }
        public int? UsuarioId { get; set; }
        public UsuarioDto? Usuario { get; set; }
    }
}
