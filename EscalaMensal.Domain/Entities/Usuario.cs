using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Entities
{
    namespace EscalaMensal.Domain.Entities
    {
        public class Usuario
        {
            public int Id { get; private set; }
            public string Nome { get; private set; }
            public string Email { get; private set; }
            public bool Ativo { get; private set; }

            public int NivelId { get; private set; }

            public int? UsuarioVinculadoId { get; private set; }
            public Usuario? UsuarioVinculado { get; private set; }

            public Usuario(string nome, string email, int nivelId)
            {
                Nome = nome;
                Email = email;
                NivelId = nivelId;
                Ativo = true;
            }

            public void VincularUsuario(int usuarioId) => UsuarioVinculadoId = usuarioId;
            public void DesvincularUsuario() => UsuarioVinculadoId = null;

            public void Ativar() => Ativo = true;
            public void Desativar() => Ativo = false;
        }

    }

}
