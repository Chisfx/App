using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;

namespace App.Application.Interfaces.Contexts
{
    /// <summary>
    /// Represents the application database context.
    /// </summary>
    public interface IApplicationDbContext
    {
        /// <summary>
        /// Gets the connection to the database.
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// Gets a value indicating whether there are any pending changes in the context.
        /// </summary>
        bool HasChanges { get; }

        /// <summary>
        /// Gets the database instance associated with the context.
        /// </summary>
        DatabaseFacade DataBase { get; }

        /// <summary>
        /// Gets the EntityEntry for the specified entity.
        /// </summary>
        /// <param name="entity">The entity object.</param>
        /// <returns>The EntityEntry for the specified entity.</returns>
        EntityEntry Entry(object entity);

        /// <summary>
        /// Saves all changes made in this context to the database asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
