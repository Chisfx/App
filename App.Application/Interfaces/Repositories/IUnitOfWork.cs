namespace App.Application.Interfaces.Repositories
{
    /// <summary>
    /// Represents a unit of work for managing database transactions.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commits the changes made in the unit of work to the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<int> Commit(CancellationToken cancellationToken);

        /// <summary>
        /// Rolls back the changes made in the unit of work.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Rollback();

        /// <summary>
        /// Begins a new database transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task BeginTransactionAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Commits the database transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CommitTransactionAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Rolls back the database transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RollbackTransactionAsync(CancellationToken cancellationToken);
    }
}
