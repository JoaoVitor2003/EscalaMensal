namespace EscalaMensal.Application.DTOs.Paroquia
{
    public class ParoquiaResumoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? Bairro { get; set; }
        public string? Endereco { get; set; }
        public int TotalMembros { get; set; }
        public bool JaParticipa { get; set; }
        public bool BloqueadoCodigo { get; set; }
        public int TentativasRestantes { get; set; } = 3;
        public bool SolicitacaoPendente { get; set; }
    }
}
