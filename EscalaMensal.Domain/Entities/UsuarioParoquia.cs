using System;

namespace EscalaMensal.Domain.Entities
{
    public class UsuarioParoquia
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public int ParoquiaId { get; set; }
        public Paroquia Paroquia { get; set; } = null!;
        public DateTime DataVinculo { get; set; } = DateTime.UtcNow;
        public int TentativasCodigoErrado { get; set; } = 0;
        public bool BloqueadoParaCodigo { get; set; } = false;
        public DateTime? DataUltimoBloqueio { get; set; }

        public UsuarioParoquia(int usuarioId, int paroquiaId)
        {
            UsuarioId = usuarioId;
            ParoquiaId = paroquiaId;
            DataVinculo = DateTime.UtcNow;
            TentativasCodigoErrado = 0;
            BloqueadoParaCodigo = false;
        }

        public void RegistrarTentativaInvalida()
        {
            TentativasCodigoErrado++;
            if (TentativasCodigoErrado >= 3)
            {
                BloqueadoParaCodigo = true;
                DataUltimoBloqueio = DateTime.UtcNow;
            }
        }

        public void ResetarTentativas()
        {
            TentativasCodigoErrado = 0;
            BloqueadoParaCodigo = false;
            DataUltimoBloqueio = null;
        }

        // Construtor para o EF Core
        protected UsuarioParoquia() { }
    }
}
