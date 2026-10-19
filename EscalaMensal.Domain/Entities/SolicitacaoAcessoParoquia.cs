using System;

namespace EscalaMensal.Domain.Entities
{
    public class SolicitacaoAcessoParoquia
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public int ParoquiaId { get; set; }
        public Paroquia Paroquia { get; set; } = null!;
        public int Status { get; set; } = 1; // 1 = Pendente, 2 = Aprovada, 3 = Rejeitada
        public string? Mensagem { get; set; }
        public DateTime DataSolicitacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataResposta { get; set; }
        public int? RespondidoPorUsuarioId { get; set; }
        public Usuario? RespondidoPorUsuario { get; set; }

        public SolicitacaoAcessoParoquia(int usuarioId, int paroquiaId, string? mensagem = null)
        {
            UsuarioId = usuarioId;
            ParoquiaId = paroquiaId;
            Mensagem = mensagem;
            Status = 1;
            DataSolicitacao = DateTime.UtcNow;
        }

        public void Aprovar(int respondidoPorUsuarioId)
        {
            Status = 2;
            RespondidoPorUsuarioId = respondidoPorUsuarioId;
            DataResposta = DateTime.UtcNow;
        }

        public void Rejeitar(int respondidoPorUsuarioId)
        {
            Status = 3;
            RespondidoPorUsuarioId = respondidoPorUsuarioId;
            DataResposta = DateTime.UtcNow;
        }

        // Construtor para o EF Core
        protected SolicitacaoAcessoParoquia() { }
    }
}
