using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Entities
{
    public class ItemEscala
    {
        public int Id { get; private set; }
        public DateTime Dia { get; set; }
        public TimeOnly? Horario { get; set; }
        public int EscalaId { get; private set; }
        public Escala Escala { get; private set; }

        public int FuncaoId { get; private set; }
        public Funcao Funcao { get; private set; }

        public int? UsuarioId { get; private set; }
        public Usuario? Usuario { get; private set; } // pode ser nulo se estiver vago

        public ItemEscala(int escalaId, int funcaoId, DateTime dia, int? usuarioId = null)
        {
            EscalaId = escalaId;
            FuncaoId = funcaoId;
            Dia = dia.Date;
            UsuarioId = usuarioId;
        }

        public void AtribuirUsuario(int usuarioId) => UsuarioId = usuarioId;
        public void RemoverUsuario() => UsuarioId = null;
    }

}
