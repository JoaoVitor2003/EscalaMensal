using System;
using EscalaMensal.Application.DTOs.Funcao;
using EscalaMensal.Application.DTOs.Usuario;

namespace EscalaMensal.Application.DTOs.HistoricoEscala
{
    public class HistoricoItemMissaDto
    {
        public int Id { get; set; }
        public int ItemMissaOriginalId { get; set; }
        public int HistoricoMissaId { get; set; }

        public int FuncaoId { get; set; }
        public FuncaoDto? Funcao { get; set; }
        public string FuncaoNome { get; set; }
        public string FuncaoAbreviacao { get; set; }

        public int? UsuarioId { get; set; }
        public UsuarioDto? Usuario { get; set; }
        public string? UsuarioNome { get; set; }

        public int Ordem { get; set; }
    }
}
