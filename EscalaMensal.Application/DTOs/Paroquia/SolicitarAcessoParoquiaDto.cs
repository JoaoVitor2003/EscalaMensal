namespace EscalaMensal.Application.DTOs.Paroquia
{
    public class SolicitarAcessoParoquiaDto
    {
        public int UsuarioId { get; set; }
        public int ParoquiaId { get; set; }
        public string? Mensagem { get; set; }
    }
}
