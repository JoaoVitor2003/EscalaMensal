using EscalaMensal.Domain.Enums;
using System;

namespace EscalaMensal.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Telefone { get; private set; }
        public string SenhaHash { get; private set; }
        public StatusAprovacaoEnum StatusAprovacao { get; private set; }
        public PerfilUsuarioEnum Perfil { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataAprovacao { get; private set; }
        public int? AprovadoPorId { get; private set; }
        public Usuario? AprovadoPor { get; private set; }

        public Usuario(string nome, string email, string telefone, string senhaHash, PerfilUsuarioEnum perfil = PerfilUsuarioEnum.Membro)
        {
            Nome = nome;
            Email = email;
            Telefone = telefone;
            SenhaHash = senhaHash;
            StatusAprovacao = StatusAprovacaoEnum.Pendente;
            Perfil = perfil;
            DataCriacao = DateTime.UtcNow;
        }

        protected Usuario() { }

        public void Aprovar(int aprovadorId)
        {
            StatusAprovacao = StatusAprovacaoEnum.Aprovado;
            DataAprovacao = DateTime.UtcNow;
            AprovadoPorId = aprovadorId;
        }

        public void Rejeitar(int aprovadorId)
        {
            StatusAprovacao = StatusAprovacaoEnum.Rejeitado;
            DataAprovacao = DateTime.UtcNow;
            AprovadoPorId = aprovadorId;
        }

        public void AtualizarSenha(string novoHash)
        {
            SenhaHash = novoHash;
        }

        public void AtualizarDados(string nome, string telefone)
        {
            Nome = nome;
            Telefone = telefone;
        }
    }
}
