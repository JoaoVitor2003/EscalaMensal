using EscalaMensal.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Entities
{
    public class ItemEscala
    {
        public int Id { get; private set; }
        public DateOnly Dia { get; set; }

        [JsonConverter(typeof(TimeOnlyHmConverter))]
        public TimeOnly? Horario { get; set; }
        public int EscalaId { get; private set; }
        public Escala? Escala { get; private set; }

        public int FuncaoId { get; private set; }
        public Funcao? Funcao { get; private set; }

        public int? UsuarioId { get; private set; }
        public Usuario? Usuario { get; private set; } // pode ser nulo se estiver vago

        public ItemEscala(int escalaId, int funcaoId, DateOnly dia, int? usuarioId = null)
        {
            EscalaId = escalaId;
            FuncaoId = funcaoId;
            Dia = dia;
            UsuarioId = usuarioId;
        }

        public void AtribuirUsuario(int usuarioId) => UsuarioId = usuarioId;
        public void RemoverUsuario() => UsuarioId = null;
    }

}
