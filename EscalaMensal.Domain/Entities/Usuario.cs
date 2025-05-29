﻿using System;
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
            public bool Ativo { get; private set; }

            public int NivelId { get; private set; }
            public Nivel Nivel { get; private set; }
            public int CargoId { get; private set; }
            public Cargo Cargo { get; private set; }

            public int? UsuarioVinculadoId { get; private set; }
            public Usuario? UsuarioVinculado { get; private set; }
            public ICollection<Restricao> Restricoes { get; private set; } = new List<Restricao>();
            public Usuario(string nome, int nivelId, int cargoId, int? usuarioVinculadoId = null)
            {
                Nome = nome;
                NivelId = nivelId;
                CargoId = cargoId;
                UsuarioVinculadoId = usuarioVinculadoId;
                Ativo = true;
            }

            public void VincularUsuario(int usuarioId) => UsuarioVinculadoId = usuarioId;
            public void DesvincularUsuario() => UsuarioVinculadoId = null;

            public void Ativar() => Ativo = true;
            public void Desativar() => Ativo = false;
        }

    }

}
