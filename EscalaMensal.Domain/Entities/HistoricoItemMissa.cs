using System;
using System.Text.Json.Serialization;

namespace EscalaMensal.Domain.Entities
{
    public class HistoricoItemMissa
    {
        public int Id { get; private set; }
        public int ItemMissaOriginalId { get; private set; }
        public int HistoricoMissaId { get; private set; }

        [JsonIgnore]
        public HistoricoMissa? HistoricoMissa { get; private set; }

        public int FuncaoId { get; private set; }
        public Funcao? Funcao { get; private set; }
        public string FuncaoNome { get; private set; }
        public string FuncaoAbreviacao { get; private set; }

        public int? UsuarioId { get; private set; }
        public Usuario? Usuario { get; private set; }
        public string? UsuarioNome { get; private set; }

        public int Ordem { get; private set; }

        public HistoricoItemMissa(int itemMissaOriginalId, int historicoMissaId, int funcaoId, string funcaoNome, string funcaoAbreviacao, int? usuarioId, string? usuarioNome, int ordem)
        {
            ItemMissaOriginalId = itemMissaOriginalId;
            HistoricoMissaId = historicoMissaId;
            FuncaoId = funcaoId;
            FuncaoNome = funcaoNome;
            FuncaoAbreviacao = funcaoAbreviacao;
            UsuarioId = usuarioId;
            UsuarioNome = usuarioNome;
            Ordem = ordem;
        }

        private HistoricoItemMissa() { }
    }
}
