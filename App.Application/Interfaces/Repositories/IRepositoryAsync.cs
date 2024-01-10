using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace App.Application.Interfaces.Repositories
{
    /// <summary>
    /// Represents a generic repository interface for asynchronous operations.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface IRepositoryAsync<T> where T : class
    {
        /// <summary>
        /// Gets the entities queryable.
        /// </summary>
        IQueryable<T> Entities { get; }

        /// <summary>
        /// Retrieves a paged response of entities.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves a paged response of entities with includes.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes);

        /// <summary>
        /// Retrieves all entities.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Retrieves all entities with includes.
        /// </summary>
        /// <param name="includes">The includes.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Retrieves all entities with a filter and includes.
        /// </summary>
        /// <param name="where">The filter expression.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Retrieves all entities with includes.
        /// </summary>
        /// <param name="includes">The includes.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<List<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes);

        /// <summary>
        /// Retrieves all entities with a filter and includes.
        /// </summary>
        /// <param name="where">The filter expression.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes);

        /// <summary>
        /// Retrieves the first or default entity with a filter.
        /// </summary>
        /// <param name="where">The filter expression.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> where);

        /// <summary>
        /// Retrieves the first or default entity with a filter and includes.
        /// </summary>
        /// <param name="where">The filter expression.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes);

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        /// <typeparam name="TKey">The type of the ID.</typeparam>
        /// <param name="Id">The ID value.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<T> GetByIdAsync<TKey>(TKey Id);

        /// <summary>
        /// Retrieves an entity by its ID with reference and collection includes.
        /// </summary>
        /// <typeparam name="TKey">The type of the ID.</typeparam>
        /// <param name="Id">The ID value.</param>
        /// <param name="Reference">The reference includes.</param>
        /// <param name="Collection">The collection includes.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<T> GetByIdAsync<TKey>(TKey Id, string[]? Reference = null, string[]? Collection = null);

        /// <summary>
        /// Retrieves an entity by its IDs.
        /// </summary>
        /// <typeparam name="TKey">The type of the IDs.</typeparam>
        /// <param name="Id">The ID values.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<T> GetByIdAsync<TKey>(params TKey[] Id);

        /// <summary>
        /// Retrieves an entity by its IDs.
        /// </summary>
        /// <param name="Id">The ID values.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<T> GetByIdAsync(params object[] Id);

        /// <summary>
        /// Retrieves a reference entity by its ID with includes.
        /// </summary>
        /// <typeparam name="TKey">The type of the ID.</typeparam>
        /// <param name="Id">The ID value.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<T> GetReferenceByIdAsync<TKey>(TKey Id, params string[] includes);

        /// <summary>
        /// Retrieves a collection entity by its ID with includes.
        /// </summary>
        /// <typeparam name="TKey">The type of the ID.</typeparam>
        /// <param name="Id">The ID value.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<T> GetCollectionByIdAsync<TKey>(TKey Id, params string[] includes);

        /// <summary>
        /// Checks if any entity satisfies the filter.
        /// </summary>
        /// <param name="where">The filter expression.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> where);

        /// <summary>
        /// Checks if any entity satisfies the filter with includes.
        /// </summary>
        /// <param name="where">The filter expression.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes);

        /// <summary>
        /// Adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Adds multiple entities.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<List<T>> AddAsync(List<T> entities);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Updates multiple existing entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(List<T> entities);

        /// <summary>
        /// Deletes an existing entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Deletes multiple existing entities.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(List<T> entities);

        /// <summary>
        /// Gets the value of the primary key for an entity.
        /// </summary>
        /// <typeparam name="K">The type of the primary key.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<K> GetValuePrimaryKey<K>(T entity);

        /// <summary>
        /// Loads an entity with reference and collection includes.
        /// </summary>
        /// <param name="entity">The entity to load.</param>
        /// <param name="reference">The reference includes.</param>
        /// <param name="collection">The collection includes.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<T> EntryLoadAsync(T entity, string? reference = null, string? collection = null);

        /// <summary>
        /// Loads an entity with reference and collection includes.
        /// </summary>
        /// <param name="entity">The entity to load.</param>
        /// <param name="reference">The reference includes.</param>
        /// <param name="collection">The collection includes.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<T> EntryLoadAsync(T entity, string[]? reference = null, string[]? collection = null);

        /// <summary>
        /// Loads a reference entity with includes.
        /// </summary>
        /// <param name="entity">The entity to load.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<T> EntryReferenceLoadAsync(T entity, params string[] includes);

        /// <summary>
        /// Loads reference entities with includes.
        /// </summary>
        /// <param name="list">The list of entities to load.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task EntryReferenceLoadAsync(List<T> list, params string[] includes);

        /// <summary>
        /// Loads a collection entity with includes.
        /// </summary>
        /// <param name="entity">The entity to load.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<T> EntryCollectionLoadAsync(T entity, params string[] includes);

        /// <summary>
        /// Loads collection entities with includes.
        /// </summary>
        /// <param name="list">The list of entities to load.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task EntryCollectionLoadAsync(List<T> list, params string[] includes);

        /// <summary>
        /// Sets the state of an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="state">The state.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SetState(T entity, EntityState state);

        /// <summary>
        /// Sets the state of multiple entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="state">The state.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SetState(List<T> entities, EntityState state);

        /// <summary>
        /// Counts the number of entities that satisfy the filter.
        /// </summary>
        /// <param name="where">The filter expression.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> where);
    }
}
