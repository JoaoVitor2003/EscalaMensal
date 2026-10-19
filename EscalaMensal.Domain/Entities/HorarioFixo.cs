using System;

namespace EscalaMensal.Domain.Entities
{
    public class HorarioFixo
    {
        public int Id { get; private set; }
        public TimeOnly Horario { get; private set; }
        public int? ParoquiaId { get; private set; }
        public Paroquia? Paroquia { get; private set; }

        public HorarioFixo(TimeOnly horario, int? paroquiaId = null)
        {
            Horario = horario;
            ParoquiaId = paroquiaId;
        }

        public void DefinirParoquia(int paroquiaId) => ParoquiaId = paroquiaId;

        // Constructor for EF Core
        protected HorarioFixo() { }
    }
}
