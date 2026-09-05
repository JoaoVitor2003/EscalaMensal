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
        [JsonIgnore]
        public Missas? Missa { get; private set; }
        public int FuncaoId { get; private set; }
        public Funcao? Funcao { get; private set; }
        public int Ordem { get; set; }
        public int? MembroId { get; private set; }
        public Membro? Membro { get; private set; }
        public ItemMissa(int missaId, int funcaoId, int? membroId = null)
        {
            MissaId = missaId;
            FuncaoId = funcaoId;
            MembroId = membroId == 0 ? null : membroId;
        }

        public ItemMissa(int id, int ordem)
        {
            Id = id;
            Ordem = ordem;
        }

        public void AtualizarOrdem(int ordem)
        {
            Ordem = ordem;
        }

        public void AtribuirMembro(int? membroId) => MembroId = (membroId == 0 ? null : membroId);
        public void RemoverMembro() => MembroId = null;
    }

}
