using App.Application.Interfaces.Contexts;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;

namespace App.Infrastructure.DbContexts
{
    /// <summary>
    /// Represents the application database context.
    /// </summary>
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for configuring the context.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets the database connection.
        /// </summary>
        public IDbConnection Connection => Database.GetDbConnection();

        /// <summary>
        /// Gets a value indicating whether there are any changes made in the context.
        /// </summary>
        public bool HasChanges => ChangeTracker.HasChanges();

        /// <summary>
        /// Gets the database facade.
        /// </summary>
        public DatabaseFacade DataBase => Database;

        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Saves the changes made in the context asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Configures the model for the context.
        /// </summary>
        /// <param name="builder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("dbo");

            builder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
