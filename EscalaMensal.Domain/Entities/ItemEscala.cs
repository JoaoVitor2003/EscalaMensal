using EscalaMensal.Domain.Entities.EscalaMensal.Domain.Entities;
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
        public int EscalaId { get; private set; }
        public Escala Escala { get; private set; }

        public int FuncaoId { get; private set; }
        public Funcao Funcao { get; private set; }

        public int? UsuarioId { get; private set; }
        public Usuario? Usuario { get; private set; } // pode ser nulo se estiver vago

        public ItemEscala(int escalaId, int funcaoId, int? usuarioId = null)
        {
            EscalaId = escalaId;
            FuncaoId = funcaoId;
            UsuarioId = usuarioId;
        }

        public void AtribuirUsuario(int usuarioId) => UsuarioId = usuarioId;
        public void RemoverUsuario() => UsuarioId = null;
    }

}
