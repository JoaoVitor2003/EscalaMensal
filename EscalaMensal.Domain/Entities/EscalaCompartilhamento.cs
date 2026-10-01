using System;

namespace EscalaMensal.Domain.Entities
{
    public class EscalaCompartilhamento
    {
        public int Id { get; set; }
        public int EscalaId { get; set; }
        public Escala Escala { get; set; } = null!;
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int Permissao { get; set; } = 1; // 1 = Editor, 2 = Visualizador
        public string CodigoAcesso { get; set; } = string.Empty;
        public int CriadoPorUsuarioId { get; set; }
        public Usuario CriadoPorUsuario { get; set; } = null!;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
