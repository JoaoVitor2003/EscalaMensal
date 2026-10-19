using System;
using System.Collections.Generic;

namespace EscalaMensal.Domain.Entities
{
    public class Paroquia
    {
        public int Id { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public string Cidade { get; private set; } = string.Empty;
        public string Estado { get; private set; } = string.Empty;
        public string? Bairro { get; private set; }
        public string? Endereco { get; private set; }
        public string CodigoAcesso { get; private set; } = string.Empty;
        public int CriadoPorUsuarioId { get; private set; }
        public Usuario CriadoPorUsuario { get; private set; } = null!;
        public DateTime DataCriacao { get; private set; } = DateTime.UtcNow;
        public bool Ativa { get; private set; } = true;

        public ICollection<UsuarioParoquia> UsuariosParoquia { get; private set; } = new List<UsuarioParoquia>();
        public ICollection<Funcao> Funcoes { get; private set; } = new List<Funcao>();
        public ICollection<Membro> Membros { get; private set; } = new List<Membro>();
        public ICollection<Escala> Escalas { get; private set; } = new List<Escala>();
        public ICollection<MissaPadrao> MissasPadrao { get; private set; } = new List<MissaPadrao>();
        public ICollection<HorarioFixo> HorariosFixos { get; private set; } = new List<HorarioFixo>();

        public Paroquia(string nome, string cidade, string estado, string? bairro, string? endereco, string codigoAcesso, int criadoPorUsuarioId)
        {
            Nome = nome;
            Cidade = cidade;
            Estado = estado;
            Bairro = bairro;
            Endereco = endereco;
            CodigoAcesso = codigoAcesso;
            CriadoPorUsuarioId = criadoPorUsuarioId;
            DataCriacao = DateTime.UtcNow;
            Ativa = true;
        }

        public void AtualizarDados(string nome, string cidade, string estado, string? bairro, string? endereco)
        {
            Nome = nome;
            Cidade = cidade;
            Estado = estado;
            Bairro = bairro;
            Endereco = endereco;
        }

        public void Desativar() => Ativa = false;
        public void Ativar() => Ativa = true;

        // Construtor para o EF Core
        protected Paroquia() { }
    }
}
