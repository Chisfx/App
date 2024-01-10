using App.Infrastructure.DbContexts;
using App.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace App.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private bool disposed;
        private IDbContextTransaction Transaction;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Rollback()
        {
            //todo
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                    _dbContext.Dispose();
                }
            }
            //dispose unmanaged resources
            disposed = true;
        }
        public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        {
            Transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }
        public async Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            if (Transaction != null)
            {
                await Transaction.CommitAsync(cancellationToken);
                await Transaction.DisposeAsync();
            }
        }
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
        {
            if (Transaction != null)
            {
                await Transaction.RollbackAsync(cancellationToken);
                await Transaction.DisposeAsync();
            }
        }
    }

}
