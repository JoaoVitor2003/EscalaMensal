using System;

namespace EscalaMensal.Domain.Entities
{
    public class MissaPadrao
    {
        public int Id { get; private set; }
        public DayOfWeek DiaSemana { get; private set; }
        public TimeOnly Horario { get; private set; }

        public MissaPadrao(DayOfWeek diaSemana, TimeOnly horario)
        {
            DiaSemana = diaSemana;
            Horario = horario;
        }

        // Construtor para o EF Core
        protected MissaPadrao() { }
    }
}
