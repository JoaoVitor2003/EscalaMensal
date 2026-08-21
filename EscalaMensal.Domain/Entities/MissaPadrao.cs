using System;

namespace EscalaMensal.Domain.Entities
{
    public class MissaPadrao
    {
        public int Id { get; private set; }
        public DayOfWeek DiaSemana { get; private set; }
        public TimeOnly Horario { get; private set; }
        public bool CriarFuncoesPadrao { get; private set; } = true;

        public MissaPadrao(DayOfWeek diaSemana, TimeOnly horario, bool criarFuncoesPadrao = true)
        {
            DiaSemana = diaSemana;
            Horario = horario;
            CriarFuncoesPadrao = criarFuncoesPadrao;
        }

        public void Atualizar(DayOfWeek diaSemana, TimeOnly horario, bool criarFuncoesPadrao)
        {
            DiaSemana = diaSemana;
            Horario = horario;
            CriarFuncoesPadrao = criarFuncoesPadrao;
        }

        // Construtor para o EF Core
        protected MissaPadrao() { }
    }
}
