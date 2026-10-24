namespace EscalaMensal.Application.DTOs.Paroquia
{
    public class ParoquiaCriarDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? Bairro { get; set; }
        public string? Endereco { get; set; }
        public int CriadoPorUsuarioId { get; set; }
        public int? ModeloBaseParoquiaId { get; set; }
    }
}
