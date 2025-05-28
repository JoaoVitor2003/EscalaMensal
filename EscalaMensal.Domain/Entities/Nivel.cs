using EscalaMensal.Domain.Entities.EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Entities
{
    public class Nivel
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }

        public ICollection<Usuario> Usuarios { get; private set; } = new List<Usuario>();

        public Nivel(string nome) => Nome = nome;
    }

}
