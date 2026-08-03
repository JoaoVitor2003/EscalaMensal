using System;

namespace EscalaMensal.Domain.Entities
{
    public class HorarioFixo
    {
        public int Id { get; private set; }
        public TimeOnly Horario { get; private set; }

        public HorarioFixo(TimeOnly horario)
        {
            Horario = horario;
        }

        // Constructor for EF Core
        protected HorarioFixo() { }
    }
}
