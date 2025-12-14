using EscalaMensal.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public bool Ativo { get; private set; }

        public CargoEnum Cargo { get; private set; }
        public NivelEnum Nivel { get; private set; }
        public TimeOnly? HoraPreferencial { get; set; }
        public bool DisponivelSabado { get; set; }
        public bool DisponivelQuarta { get; set; }
        public bool DisponivelQuinta { get; set; }
        public int? UsuarioVinculadoId { get; private set; }
        public Usuario? UsuarioVinculado { get; private set; }
        public int? DiasEscalados { get; set; }
        public ICollection<Restricao> Restricoes { get; private set; } = new List<Restricao>();
        public Usuario(string nome, NivelEnum nivel, CargoEnum cargo, int? usuarioVinculadoId = null)
        {
            Nome = nome;
            Nivel = nivel;
            Cargo = cargo;
            UsuarioVinculadoId = usuarioVinculadoId;
            Ativo = true;
        }

        public void VincularUsuario(int usuarioId) => UsuarioVinculadoId = usuarioId;
        public void DesvincularUsuario() => UsuarioVinculadoId = null;

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;
    }

}
