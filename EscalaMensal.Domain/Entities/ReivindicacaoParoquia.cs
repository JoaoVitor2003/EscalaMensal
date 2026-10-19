using System;

namespace EscalaMensal.Domain.Entities
{
    public class ReivindicacaoParoquia
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public int ParoquiaId { get; set; }
        public Paroquia Paroquia { get; set; } = null!;
        public string Justificativa { get; set; } = string.Empty;
        public string TelefoneContato { get; set; } = string.Empty;
        public int Status { get; set; } = 1; // 1 = Pendente, 2 = Aprovada, 3 = Rejeitada
        public DateTime DataSolicitacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataResposta { get; set; }

        public ReivindicacaoParoquia(int usuarioId, int paroquiaId, string justificativa, string telefoneContato)
        {
            UsuarioId = usuarioId;
            ParoquiaId = paroquiaId;
            Justificativa = justificativa;
            TelefoneContato = telefoneContato;
            Status = 1;
            DataSolicitacao = DateTime.UtcNow;
        }

        public void Aprovar()
        {
            Status = 2;
            DataResposta = DateTime.UtcNow;
        }

        public void Rejeitar()
        {
            Status = 3;
            DataResposta = DateTime.UtcNow;
        }

        // Construtor para o EF Core
        protected ReivindicacaoParoquia() { }
    }
}
