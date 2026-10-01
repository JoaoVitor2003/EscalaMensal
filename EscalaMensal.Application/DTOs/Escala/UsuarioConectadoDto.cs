using System;

namespace EscalaMensal.Application.DTOs.Escala
{
    public class UsuarioConectadoDto
    {
        public string ConnectionId { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Perfil { get; set; } = "Normal";
        public string Permissao { get; set; } = "Editor"; // "Editor" ou "Visualizador"
        public DateTime HoraEntrada { get; set; } = DateTime.UtcNow;
    }

    public class EscalaCompartilhamentoDto
    {
        public int Id { get; set; }
        public int EscalaId { get; set; }
        public int? UsuarioId { get; set; }
        public string? UsuarioNome { get; set; }
        public string? UsuarioEmail { get; set; }
        public int Permissao { get; set; } // 1: Editor, 2: Visualizador
        public string CodigoAcesso { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
    }

    public class CompartilharUsuarioRequest
    {
        public int UsuarioId { get; set; }
        public int Permissao { get; set; } = 1; // 1 = Editor, 2 = Visualizador
        public int? CriadoPorUsuarioId { get; set; }
    }

    public class ValidarAcessoEscalaRespostaDto
    {
        public bool TemAcesso { get; set; }
        public string Permissao { get; set; } = "Visualizador"; // "Editor" ou "Visualizador"
        public string Mensagem { get; set; } = string.Empty;
    }

    public class EscalaCompartilhadaResumoDto
    {
        public int EscalaId { get; set; }
        public DateOnly DataInicio { get; set; }
        public DateOnly DataFim { get; set; }
        public int LimitePermitido { get; set; }
        public string Permissao { get; set; } = "Visualizador"; // "Editor" ou "Visualizador"
        public int TotalUsuarios { get; set; }
        public List<string> NomesUsuariosCompartilhados { get; set; } = new();
        public string CodigoAcesso { get; set; } = string.Empty;
        public bool CompartilhadoPorMim { get; set; }
        public DateTime? DataCompartilhamento { get; set; }
    }
}
