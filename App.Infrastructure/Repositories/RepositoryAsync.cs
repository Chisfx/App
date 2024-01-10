using App.Infrastructure.DbContexts;
using App.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace App.Infrastructure.Repositories
{
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        public RepositoryAsync(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<T> Entities => _dbContext.Set<T>();
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
        public async Task<List<T>> GetAllAsync()
        {
            var result = await _dbContext.Set<T>().ToListAsync();
            return result;
        }
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
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> where)
        {
            var result = await _dbContext.Set<T>().FirstOrDefaultAsync(where);
            return result;
        }
        public async Task<T> GetByIdAsync<TKey>(TKey Id)
        {
            var result = await _dbContext.Set<T>().FindAsync(Id);
            return result;
        }
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
        public async Task<T> GetReferenceByIdAsync<TKey>(TKey Id, params string[] includes)
        {
            return await GetByIdAsync(Id, Reference: includes);
        }
        public async Task<T> GetCollectionByIdAsync<TKey>(TKey Id, params string[] includes)
        {
            return await GetByIdAsync(Id, Collection: includes);
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> where)
        {
            return await _dbContext.Set<T>().AnyAsync(where);
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }
        public async Task<List<T>> AddAsync(List<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            return entities;
        }
        public Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }
        public async Task UpdateAsync(List<T> entities)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity);
            }
        }
        public Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }
        public Task DeleteAsync(List<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }
        public async Task<K> GetValuePrimaryKey<K>(T entity)
        {
            var properties = _dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;
            var keyName = properties.Select(x => x.Name).Single();
            var type = properties.Select(x => x.ClrType).Single();
            var value = entity.GetType().GetProperty(keyName).GetValue(entity, null);
            return await Task.FromResult((K)Convert.ChangeType(value, type)); ;
        }
        public async Task<List<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsNoTracking();
            query = includes(query).AsNoTracking();
            //var str1 = query.ToQueryString();
            var result = await query.ToListAsync();
            return result;
        }
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
        {
            IQueryable<T> query = _dbContext.Set<T>().Where(where).AsNoTracking();
            query = includes(query).AsNoTracking();
            //var str1 = query.ToQueryString();
            var result = await query.FirstOrDefaultAsync();
            return result;
        }
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
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
        {
            IQueryable<T> query = _dbContext.Set<T>().Where(where).AsNoTracking();
            query = includes(query).AsNoTracking();
            //var str1 = query.ToQueryString();
            var result = await query.ToListAsync();
            return result;
        }

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

        public async Task<T> EntryReferenceLoadAsync(T entity, params string[] includes)
        {
            return await EntryLoadAsync(entity, reference: includes);
        }

        public async Task<T> EntryCollectionLoadAsync(T entity, params string[] includes)
        {
            return await EntryLoadAsync(entity, collection: includes);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            query = includes(query).AsNoTracking();
            return await query.AnyAsync(where);
        }

        public async Task<T> GetByIdAsync<TKey>(params TKey[] Id)
        {
            var result = await _dbContext.Set<T>().FindAsync(Id);
            return result;
        }

        public async Task<T> GetByIdAsync(params object[] Id)
        {
            var result = await _dbContext.Set<T>().FindAsync(Id);
            return result;
        }

        public Task SetState(T entity, EntityState state)
        {
            _dbContext.Entry(entity).State = state;
            return Task.CompletedTask;
        }

        public async Task SetState(List<T> entities, EntityState state)
        {
            foreach (var entity in entities)
            {
                await SetState(entity, state);
            }
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> where)
        {
            var result = await _dbContext.Set<T>().Where(where).ToListAsync();
            return result != null ? result.Count : 0;
        }

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
