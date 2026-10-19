using System;
using System.Collections.Generic;     
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Entities
{
    public class Escala
    {
        public int Id { get; private set; }
        public DateOnly DataInicio { get; private set; }
        public DateOnly DataFim { get; private set; }
        public int LimitePermitido { get; private set; }
        public int? CriadoPorUsuarioId { get; private set; }
        public Usuario? CriadoPorUsuario { get; private set; }
        public int? ParoquiaId { get; private set; }
        public Paroquia? Paroquia { get; private set; }
        public ICollection<Missas> Missas { get; private set; } = new List<Missas>();

        public Escala(DateOnly dataInicio, DateOnly dataFim, int limitePermitido, int? criadoPorUsuarioId = null, int? paroquiaId = null)
        {
            DataInicio = dataInicio;
            DataFim = dataFim;
            LimitePermitido = limitePermitido;
            CriadoPorUsuarioId = criadoPorUsuarioId;
            ParoquiaId = paroquiaId;
        }

        public void DefinirCriador(int usuarioId)
        {
            CriadoPorUsuarioId = usuarioId;
        }

        public void DefinirParoquia(int paroquiaId)
        {
            ParoquiaId = paroquiaId;
        }

        private Escala() { }

        public void Atualizar(DateOnly dataInicio, DateOnly dataFim, int limitePermitido)
        {
            if (dataInicio > dataFim)
            {
                throw new Exceptions.DomainException("A data de início não pode ser maior que a data de fim."); 
            }

            if (Missas.Any(m => m.Dia < dataInicio || m.Dia > dataFim))
            {
                throw new Exceptions.DomainException("Não é possível atualizar a escala pois existem missas com datas fora do novo período definido.");
            }

            var maxEscalado = Missas
                .SelectMany(m => m.ItensMissa)
                .Where(i => i.MembroId != null)
                .GroupBy(i => i.MembroId)
                .Select(g => g.Count())
                .DefaultIfEmpty(0)
                .Max();

            if (limitePermitido < maxEscalado)
            {
                throw new Exceptions.DomainException($"Não é possível reduzir o limite máximo para {limitePermitido}, pois existem membros escalados {maxEscalado} vezes.");
            }

            DataInicio = dataInicio;
            DataFim = dataFim;
            LimitePermitido = limitePermitido;
        }

        public Missas AdicionarMissa(DateOnly dia, TimeOnly horario)
        {
            if (dia < DataInicio || dia > DataFim)
            {
                throw new Exceptions.DomainException("A data da missa deve estar dentro do período da escala.");
            }

            var missa = new Missas(dia, horario, this.Id);
            Missas.Add(missa);
            return missa;
        }
    }

}
