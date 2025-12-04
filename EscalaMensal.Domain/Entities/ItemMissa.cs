using EscalaMensal.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Entities
{
    public class ItemMissa
    {
        public int Id { get; private set; }
        public int MissaId { get; private set; }
        public Missas? Missas { get; private set; }

        public int? UsuarioId { get; private set; }
        public Usuario? Usuario { get; private set; } // pode ser nulo se estiver vago

        public ItemMissa(int usuarioId, int missaId)
        {
            UsuarioId = usuarioId;
            MissaId = missaId;
        }

        public void AtribuirUsuario(int usuarioId) => UsuarioId = usuarioId;
        public void RemoverUsuario() => UsuarioId = null;
    }

}
