using System;

namespace EscalaMensal.Application.DTOs.Paroquia
{
    public class SolicitacaoAcessoParoquiaDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNome { get; set; } = string.Empty;
        public string UsuarioEmail { get; set; } = string.Empty;
        public int ParoquiaId { get; set; }
        public string ParoquiaNome { get; set; } = string.Empty;
        public int Status { get; set; } // 1 = Pendente, 2 = Aprovada, 3 = Rejeitada
        public string? Mensagem { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataResposta { get; set; }
        public int? RespondidoPorUsuarioId { get; set; }
        public string? RespondidoPorUsuarioNome { get; set; }
    }
}
