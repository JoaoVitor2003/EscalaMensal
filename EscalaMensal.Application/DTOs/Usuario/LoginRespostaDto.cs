using EscalaMensal.Domain.Enums;

namespace EscalaMensal.Application.DTOs.Usuario
{
    public class LoginRespostaDto
    {
        public bool Sucesso { get; set; }
        public string? Token { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public StatusAprovacaoEnum? Status { get; set; }
        public UsuarioRespostaDto? Usuario { get; set; }
    }
}
