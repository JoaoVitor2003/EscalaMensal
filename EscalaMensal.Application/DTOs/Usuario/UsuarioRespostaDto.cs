using EscalaMensal.Domain.Enums;
using System;

namespace EscalaMensal.Application.DTOs.Usuario
{
    public class UsuarioRespostaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public StatusAprovacaoEnum StatusAprovacao { get; set; }
        public PerfilUsuarioEnum Perfil { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
