using EscalaMensal.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Entities
{
    public class Membro
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public bool Ativo { get; private set; }

        public CargoEnum Cargo { get; private set; }
        public NivelEnum Nivel { get; private set; }
        public string HorasPreferenciaisRaw { get; set; } = "";

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public List<TimeOnly> HorasPreferenciais
        {
            get => string.IsNullOrEmpty(HorasPreferenciaisRaw)
                ? new List<TimeOnly>()
                : HorasPreferenciaisRaw.Split(',').Select(TimeOnly.Parse).ToList();
            set => HorasPreferenciaisRaw = string.Join(",", value.Select(t => t.ToString("HH:mm")));
        }
        public string DiasDisponiveisRaw { get; set; } = "";

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public List<DayOfWeek> DiasDisponiveis
        {
            get => string.IsNullOrEmpty(DiasDisponiveisRaw)
                ? new List<DayOfWeek>()
                : DiasDisponiveisRaw.Split(',').Select(Enum.Parse<DayOfWeek>).ToList();
            set => DiasDisponiveisRaw = string.Join(",", value);
        }
        public int? MembroVinculadoId { get; private set; }
        public Membro? MembroVinculado { get; private set; }
        public int? DiasEscalados { get; set; }
        public ICollection<Restricao> Restricoes { get; private set; } = new List<Restricao>();
        public Membro(string nome, NivelEnum nivel, CargoEnum cargo, int? membroVinculadoId = null)
        {
            Nome = nome;
            Nivel = nivel;
            Cargo = cargo;
            MembroVinculadoId = membroVinculadoId;
            Ativo = true;
        }
        public Membro() { }

        public void VincularMembro(int membroId) => MembroVinculadoId = membroId;
        public void DesvincularMembro() => MembroVinculadoId = null;

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;
    }
}
