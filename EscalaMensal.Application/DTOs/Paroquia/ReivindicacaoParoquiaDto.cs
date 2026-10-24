using System;

namespace EscalaMensal.Application.DTOs.Paroquia
{
    public class ReivindicacaoParoquiaDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNome { get; set; } = string.Empty;
        public int ParoquiaId { get; set; }
        public string ParoquiaNome { get; set; } = string.Empty;
        public string Justificativa { get; set; } = string.Empty;
        public string TelefoneContato { get; set; } = string.Empty;
        public int Status { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataResposta { get; set; }
    }
}
