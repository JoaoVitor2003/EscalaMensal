using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Entities
{
    public class Missas
    {
        public int Id { get; private set; }
        public DateOnly Dia { get; private set; }
        public TimeOnly Horario { get; private set; }
        public int FuncaoId { get; private set; }
        public Funcao? Funcao { get; private set; }
        public int EscalaId { get; private set; }
        public Escala? Escala { get; private set; }

        public ICollection<ItemMissa> Itens { get; private set; } = new List<ItemMissa>();

        public Missas(DateOnly dia, TimeOnly horario, int escalaId)
        {
            Dia = dia;
            Horario = horario;
            EscalaId = escalaId;
        }

        public ItemMissa AdicionarFuncao(int funcaoId)
        {
            var item = new ItemMissa(funcaoId, this.Id);
            Itens.Add(item);
            return item;
        }
    }
}
