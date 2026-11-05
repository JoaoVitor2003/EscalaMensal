using System;

namespace EscalaMensal.Application.DTOs.Escala
{
    public class GerarEscalaAleatoriaRequestDto
    {
        public int? UsuarioId { get; set; }
        public int? Perfil { get; set; }
        public bool ApenasVagasVazias { get; set; } = true;
        public bool RespeitarRestricoes { get; set; } = true;
        public bool RespeitarLimite { get; set; } = true;
        public bool EvitarMesmoDia { get; set; } = true;
        public bool AlocarIrmaosJuntos { get; set; } = true;
    }

    public class GerarEscalaAleatoriaResultadoDto
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public int TotalAlocados { get; set; }
        public int TotalVagas { get; set; }
        public int VagasRestantes { get; set; }
    }
}
