using EscalaMensal.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Entities
{
    public class Funcao
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Abreviacao { get; set; }

        public CargoEnum Cargo { get; set; }
        public NivelEnum NivelMinimo { get; set; }
        public bool Obrigatoria { get; set; }

        public Funcao(string nome) => Nome = nome;

        public Funcao(int id, string nome, bool obrigatoria)
        {
            Id = id;
            Nome = nome;
            Obrigatoria = obrigatoria;
        }
    }

}
