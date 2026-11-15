using System;
using System.Threading;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Interfaces
{
    /// <summary>
    /// Contrato de Unit of Work para proteção e garantia de atomicidade (ACID) em transações no banco de dados.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Salva todas as alterações pendentes no contexto do banco de dados.
        /// </summary>
        Task<int> CommitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Inicia uma transação explícita caso não haja uma ativa.
        /// </summary>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Comita a transação ativa confirmando todas as alterações no banco de dados.
        /// </summary>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Desfaz (rollback) todas as operações realizadas dentro da transação ativa.
        /// </summary>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executa uma ação de forma 100% transacional e atômica com estratégia de resiliência e rollback automático em falhas.
        /// </summary>
        Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executa uma função com retorno de forma 100% transacional e atômica com estratégia de resiliência e rollback automático em falhas.
        /// </summary>
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);
    }
}
