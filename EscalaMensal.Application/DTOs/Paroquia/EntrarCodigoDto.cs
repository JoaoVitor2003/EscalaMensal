namespace EscalaMensal.Application.DTOs.Paroquia
{
    public class EntrarCodigoDto
    {
        public int UsuarioId { get; set; }
        public int ParoquiaId { get; set; }
        public string CodigoAcesso { get; set; } = string.Empty;
    }
}
