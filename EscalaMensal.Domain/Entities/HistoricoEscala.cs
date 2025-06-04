using EscalaMensal.Domain.Entities.EscalaMensal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Entities
{
    public class HistoricoEscala
    {
        public int Id { get; private set; }

        public DateTime Data { get; private set; }

        public int UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; }

        public int FuncaoId { get; private set; }
        public Funcao Funcao { get; private set; }


        public HistoricoEscala(int usuarioId, int funcaoId, DateTime data)
        {
            UsuarioId = usuarioId;
            FuncaoId = funcaoId;
            Data = data;
        }
    }

}
