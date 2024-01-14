using App.Infrastructure.DbContexts;
using App.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace App.Infrastructure.Repositories
{
    /// <summary>
    /// Represents a generic asynchronous repository implementation.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryAsync{T}"/> class.
        /// </summary>
        /// <param name="dbContext">The application database context.</param>
        public RepositoryAsync(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets the entities of type <typeparamref name="T"/>.
        /// </summary>
        public IQueryable<T> Entities => _dbContext.Set<T>();

        /// <summary>
        /// Retrieves a paged response of entities of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A list of entities.</returns>
        public async Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            var result = await _dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            return result;
        }

        /// <summary>
        /// Retrieves all entities of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>A list of entities.</returns>
        public async Task<List<T>> GetAllAsync()
        {
            var result = await _dbContext.Set<T>().ToListAsync();
            return result;
        }

        /// <summary>
        /// Retrieves all entities of type <typeparamref name="T"/> with the specified includes.
        /// </summary>
        /// <param name="includes">The includes.</param>
        /// <returns>A list of entities.</returns>
        public async Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>().Include(includes[0]);
            foreach (var include in includes.Skip(1))
            {
                query = query.Include(include);
            }
            var result = await query.ToListAsync();
            return result;
        }

        /// <summary>
        /// Retrieves all entities of type <typeparamref name="T"/> that satisfy the specified condition with the specified includes.
        /// </summary>
        /// <param name="where">The condition.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>A list of entities.</returns>
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>().Where(where);

            if (includes != null && includes.Length > 0)
            {
                query = query.Include(includes[0]);
                foreach (var include in includes.Skip(1))
                {
                    query = query.Include(include);
                }
            }
            var result = await query.ToListAsync();
            return result;
        }

        /// <summary>
        /// Retrieves the first entity of type <typeparamref name="T"/> that satisfies the specified condition.
        /// </summary>
        /// <param name="where">The condition.</param>
        /// <returns>The first entity that satisfies the condition.</returns>
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> where)
        {
            var result = await _dbContext.Set<T>().FirstOrDefaultAsync(where);
            return result;
        }
        /// <summary>
        /// Retrieves an entity of type <typeparamref name="T"/> by its ID.
        /// </summary>
        /// <typeparam name="TKey">The type of the ID.</typeparam>
        /// <param name="Id">The ID of the entity.</param>
        /// <returns>The entity with the specified ID.</returns>
        public async Task<T> GetByIdAsync<TKey>(TKey Id)
        {
            var result = await _dbContext.Set<T>().FindAsync(Id);
            return result;
        }

        /// <summary>
        /// Retrieves an entity of type <typeparamref name="T"/> by its ID with the specified references and collections.
        /// </summary>
        /// <typeparam name="TKey">The type of the ID.</typeparam>
        /// <param name="Id">The ID of the entity.</param>
        /// <param name="Reference">The references to include.</param>
        /// <param name="Collection">The collections to include.</param>
        /// <returns>The entity with the specified ID.</returns>
        public async Task<T> GetByIdAsync<TKey>(TKey Id, string[] Reference = null, string[] Collection = null)
        {
            var result = await _dbContext.Set<T>().FindAsync(Id);

            if (Reference != null)
            {
                foreach (var include in Reference)
                {
                    await _dbContext.Entry(result).Reference(include).LoadAsync();
                }
            }

            if (Collection != null)
            {
                foreach (var include in Collection)
                {
                    await _dbContext.Entry(result).Collection(include).LoadAsync();
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieves an entity of type <typeparamref name="T"/> by its ID with the specified references.
        /// </summary>
        /// <typeparam name="TKey">The type of the ID.</typeparam>
        /// <param name="Id">The ID of the entity.</param>
        /// <param name="includes">The references to include.</param>
        /// <returns>The entity with the specified ID.</returns>
        public async Task<T> GetReferenceByIdAsync<TKey>(TKey Id, params string[] includes)
        {
            return await GetByIdAsync(Id, Reference: includes);
        }

        /// <summary>
        /// Retrieves an entity of type <typeparamref name="T"/> by its ID with the specified collections.
        /// </summary>
        /// <typeparam name="TKey">The type of the ID.</typeparam>
        /// <param name="Id">The ID of the entity.</param>
        /// <param name="includes">The collections to include.</param>
        /// <returns>The entity with the specified ID.</returns>
        public async Task<T> GetCollectionByIdAsync<TKey>(TKey Id, params string[] includes)
        {
            return await GetByIdAsync(Id, Collection: includes);
        }
        /// <summary>
        /// Checks if any entity of type <typeparamref name="T"/> satisfies the specified condition.
        /// </summary>
        /// <param name="where">The condition.</param>
        /// <returns>True if any entity satisfies the condition, otherwise false.</returns>
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> where)
        {
            return await _dbContext.Set<T>().AnyAsync(where);
        }

        /// <summary>
        /// Adds a new entity of type <typeparamref name="T"/> to the database.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Adds a list of entities of type <typeparamref name="T"/> to the database.
        /// </summary>
        /// <param name="entities">The list of entities to add.</param>
        /// <returns>The added entities.</returns>
        public async Task<List<T>> AddAsync(List<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        /// <summary>
        /// Updates an existing entity of type <typeparamref name="T"/> in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).CurrentValues.SetValues(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates a list of existing entities of type <typeparamref name="T"/> in the database.
        /// </summary>
        /// <param name="entities">The list of entities to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateAsync(List<T> entities)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity);
            }
        }

        /// <summary>
        /// Deletes an existing entity of type <typeparamref name="T"/> from the database.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes a list of existing entities of type <typeparamref name="T"/> from the database.
        /// </summary>
        /// <param name="entities">The list of entities to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task DeleteAsync(List<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets the value of the primary key for an entity of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="K">The type of the primary key.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>The value of the primary key.</returns>
        public async Task<K> GetValuePrimaryKey<K>(T entity)
        {
            var properties = _dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;
            var keyName = properties.Select(x => x.Name).Single();
            var type = properties.Select(x => x.ClrType).Single();
            var value = entity.GetType().GetProperty(keyName).GetValue(entity, null);
            return await Task.FromResult((K)Convert.ChangeType(value, type));
        }

        /// <summary>
        /// Retrieves all entities of type <typeparamref name="T"/> with the specified includes.
        /// </summary>
        /// <param name="includes">The includes.</param>
        /// <returns>A list of entities.</returns>
        public async Task<List<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsNoTracking();
            query = includes(query).AsNoTracking();
            var result = await query.ToListAsync();
            return result;
        }

        /// <summary>
        /// Retrieves the first entity of type <typeparamref name="T"/> that satisfies the specified condition with the specified includes.
        /// </summary>
        /// <param name="where">The condition.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>The first entity that satisfies the condition.</returns>
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
        {
            IQueryable<T> query = _dbContext.Set<T>().Where(where).AsNoTracking();
            query = includes(query).AsNoTracking();
            var result = await query.FirstOrDefaultAsync();
            return result;
        }
        /// <summary>
        /// Retrieves a paged response of entities.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="includes">The includes to be included in the query.</param>
        /// <returns>A list of entities.</returns>
        public async Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
        {
            IQueryable<T> query = _dbContext.Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking();
            query = includes(query).AsNoTracking();
            var result = await query.ToListAsync();
            return result;
        }

        /// <summary>
        /// Retrieves all entities that satisfy the specified condition.
        /// </summary>
        /// <param name="where">The condition to filter the entities.</param>
        /// <param name="includes">The includes to be included in the query.</param>
        /// <returns>A list of entities.</returns>
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
        {
            IQueryable<T> query = _dbContext.Set<T>().Where(where).AsNoTracking();
            query = includes(query).AsNoTracking();
            var result = await query.ToListAsync();
            return result;
        }

        /// <summary>
        /// Loads the specified reference and collection of an entity.
        /// </summary>
        /// <param name="entity">The entity to load the reference and collection for.</param>
        /// <param name="reference">The reference to load.</param>
        /// <param name="collection">The collection to load.</param>
        /// <returns>The loaded entity.</returns>
        public async Task<T> EntryLoadAsync(T entity, string reference = null, string collection = null)
        {
            if (reference != null)
            {
                await _dbContext.Entry(entity).Reference(reference).LoadAsync();
            }

            if (collection != null)
            {
                await _dbContext.Entry(entity).Collection(collection).LoadAsync();
            }

            return entity;
        }

        /// <summary>
        /// Loads the specified references and collections of an entity.
        /// </summary>
        /// <param name="entity">The entity to load the references and collections for.</param>
        /// <param name="reference">The references to load.</param>
        /// <param name="collection">The collections to load.</param>
        /// <returns>The loaded entity.</returns>
        public async Task<T> EntryLoadAsync(T entity, string[] reference = null, string[] collection = null)
        {
            if (reference != null)
            {
                foreach (var include in reference)
                {
                    await _dbContext.Entry(entity).Reference(include).LoadAsync();
                }
            }

            if (collection != null)
            {
                foreach (var include in collection)
                {
                    await _dbContext.Entry(entity).Collection(include).LoadAsync();
                }
            }

            return entity;
        }

        /// <summary>
        /// Loads the specified references of an entity.
        /// </summary>
        /// <param name="entity">The entity to load the references for.</param>
        /// <param name="includes">The references to load.</param>
        /// <returns>The loaded entity.</returns>
        public async Task<T> EntryReferenceLoadAsync(T entity, params string[] includes)
        {
            return await EntryLoadAsync(entity, reference: includes);
        }

        /// <summary>
        /// Loads the specified collections of an entity.
        /// </summary>
        /// <param name="entity">The entity to load the collections for.</param>
        /// <param name="includes">The collections to load.</param>
        /// <returns>The loaded entity.</returns>
        public async Task<T> EntryCollectionLoadAsync(T entity, params string[] includes)
        {
            return await EntryLoadAsync(entity, collection: includes);
        }

        /// <summary>
        /// Checks if any entity satisfies the specified condition.
        /// </summary>
        /// <param name="where">The condition to check.</param>
        /// <param name="includes">The includes to be included in the query.</param>
        /// <returns>True if any entity satisfies the condition, otherwise false.</returns>
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            query = includes(query).AsNoTracking();
            return await query.AnyAsync(where);
        }

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        /// <typeparam name="TKey">The type of the ID.</typeparam>
        /// <param name="Id">The ID of the entity.</param>
        /// <returns>The entity with the specified ID.</returns>
        public async Task<T> GetByIdAsync<TKey>(params TKey[] Id)
        {
            var result = await _dbContext.Set<T>().FindAsync(Id);
            return result;
        }

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        /// <param name="Id">The ID of the entity.</param>
        /// <returns>The entity with the specified ID.</returns>
        public async Task<T> GetByIdAsync(params object[] Id)
        {
            var result = await _dbContext.Set<T>().FindAsync(Id);
            return result;
        }

        /// <summary>
        /// Sets the state of an entity.
        /// </summary>
        /// <param name="entity">The entity to set the state for.</param>
        /// <param name="state">The state to set.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SetState(T entity, EntityState state)
        {
            _dbContext.Entry(entity).State = state;
            return Task.CompletedTask;
        }
        /// <summary>
        /// Sets the state of a list of entities.
        /// </summary>
        /// <param name="entities">The list of entities.</param>
        /// <param name="state">The state to set.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SetState(List<T> entities, EntityState state)
        {
            foreach (var entity in entities)
            {
                await SetState(entity, state);
            }
        }

        /// <summary>
        /// Counts the number of entities that satisfy the specified condition.
        /// </summary>
        /// <param name="where">The condition to satisfy.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<int> CountAsync(Expression<Func<T, bool>> where)
        {
            var result = await _dbContext.Set<T>().Where(where).ToListAsync();
            return result != null ? result.Count : 0;
        }

        /// <summary>
        /// Loads the specified collections for a list of entities.
        /// </summary>
        /// <param name="list">The list of entities.</param>
        /// <param name="includes">The collections to load.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task EntryCollectionLoadAsync(List<T> list, params string[] includes)
        {
            foreach (var entity in list)
            {
                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        await _dbContext.Entry(entity).Collection(include).LoadAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Loads the specified references for a list of entities.
        /// </summary>
        /// <param name="list">The list of entities.</param>
        /// <param name="includes">The references to load.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task EntryReferenceLoadAsync(List<T> list, params string[] includes)
        {
            foreach (var entity in list)
            {
                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        await _dbContext.Entry(entity).Reference(include).LoadAsync();
                    }
                }
            }
        }
    }
}
