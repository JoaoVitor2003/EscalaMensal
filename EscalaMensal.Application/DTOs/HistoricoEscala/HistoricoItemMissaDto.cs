using System;
using EscalaMensal.Application.DTOs.Funcao;
using EscalaMensal.Application.DTOs.Membro;

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

        public int? MembroId { get; set; }
        public MembroDto? Membro { get; set; }
        public string? MembroNome { get; set; }

        public int Ordem { get; set; }
    }
}
