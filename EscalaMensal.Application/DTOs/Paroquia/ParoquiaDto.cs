using System;

namespace EscalaMensal.Application.DTOs.Paroquia
{
    public class ParoquiaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? Bairro { get; set; }
        public string? Endereco { get; set; }
        public string CodigoAcesso { get; set; } = string.Empty;
        public int CriadoPorUsuarioId { get; set; }
        public string? CriadoPorUsuarioNome { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Ativa { get; set; }
        public bool EhCriador { get; set; }
        public int TentativasRestantes { get; set; } = 3;
        public bool BloqueadoParaCodigo { get; set; }
    }
}
