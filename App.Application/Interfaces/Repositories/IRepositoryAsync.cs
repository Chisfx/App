using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace App.Application.Interfaces.Repositories
{
    public interface IRepositoryAsync<T> where T : class
    {
        IQueryable<T> Entities { get; }
        Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize);
        Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> where);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes);
        Task<T> GetByIdAsync<TKey>(TKey Id);
        Task<T> GetByIdAsync<TKey>(TKey Id, string[]? Reference = null, string[]? Collection = null);
        Task<T> GetByIdAsync<TKey>(params TKey[] Id);
        Task<T> GetByIdAsync(params object[] Id);
        Task<T> GetReferenceByIdAsync<TKey>(TKey Id, params string[] includes);
        Task<T> GetCollectionByIdAsync<TKey>(TKey Id, params string[] includes);
        Task<bool> AnyAsync(Expression<Func<T, bool>> where);
        Task<bool> AnyAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes);
        Task<T> AddAsync(T entity);
        Task<List<T>> AddAsync(List<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateAsync(List<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteAsync(List<T> entities);
        Task<K> GetValuePrimaryKey<K>(T entity);
        Task<T> EntryLoadAsync(T entity, string? reference = null, string? collection = null);
        Task<T> EntryLoadAsync(T entity, string[]? reference = null, string[]? collection = null);
        Task<T> EntryReferenceLoadAsync(T entity, params string[] includes);
        Task EntryReferenceLoadAsync(List<T> list, params string[] includes);
        Task<T> EntryCollectionLoadAsync(T entity, params string[] includes);
        Task EntryCollectionLoadAsync(List<T> list, params string[] includes);
        Task SetState(T entity, EntityState state);
        Task SetState(List<T> entities, EntityState state);
        Task<int> CountAsync(Expression<Func<T, bool>> where);
    }

}
