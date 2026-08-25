using System;
using System.Collections.Generic;

namespace EscalaMensal.Domain.Entities
{
    public class MissaPadrao
    {
        public int Id { get; private set; }
        public DayOfWeek DiaSemana { get; private set; }
        public TimeOnly Horario { get; private set; }
        public bool CriarFuncoesPadrao { get; private set; } = true;
        public ICollection<Funcao> Funcoes { get; private set; } = new List<Funcao>();

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

        public void AtualizarFuncoes(List<Funcao> funcoes)
        {
            Funcoes.Clear();
            foreach (var f in funcoes)
            {
                Funcoes.Add(f);
            }
        }

        // Construtor para o EF Core
        protected MissaPadrao() { }
    }
}
