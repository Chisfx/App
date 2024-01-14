using App.Infrastructure.DbContexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using App.SharedTest;
using System.Data.Common;
namespace App.IntegrationTest
{
    public class SharedDatabaseFixture : IDisposable
    {
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        private readonly string dbName = "IntegrationTestsDatabase.db";

        public SharedDatabaseFixture()
        {
            Connection = new SqliteConnection($"Filename={dbName}");

            Seed();

            Connection.Open();
        }

        public DbConnection Connection { get; }

        public ApplicationDbContext CreateContext(DbTransaction? transaction = null)
        {
            var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(Connection).Options);

            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        private void Seed()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        Database.SeedData(context);
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public void Dispose() => Connection.Dispose();
    }

}
