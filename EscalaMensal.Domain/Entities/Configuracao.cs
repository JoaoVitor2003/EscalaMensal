using System.ComponentModel.DataAnnotations;

namespace EscalaMensal.Domain.Entities
{
    public class Configuracao
    {
        [Key]
        public string Chave { get; private set; }
        public string Valor { get; private set; }

        public Configuracao(string chave, string valor)
        {
            Chave = chave;
            Valor = valor;
        }

        public void AtualizarValor(string valor)
        {
            Valor = valor;
        }
    }
}
