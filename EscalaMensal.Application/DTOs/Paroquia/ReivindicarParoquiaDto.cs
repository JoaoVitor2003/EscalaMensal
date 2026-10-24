namespace EscalaMensal.Application.DTOs.Paroquia
{
    public class ReivindicarParoquiaDto
    {
        public int UsuarioId { get; set; }
        public int ParoquiaId { get; set; }
        public string Justificativa { get; set; } = string.Empty;
        public string TelefoneContato { get; set; } = string.Empty;
    }
}
