using App.Domain.Entities;
using App.Infrastructure.DbContexts;
using Bogus;
using Bogus.Extensions;
namespace App.SharedTest
{
    public static class Database
    {
        public static void SeedData(ApplicationDbContext context)
        {
            context.Users.RemoveRange(context.Users);

            var user = new Faker<User>()
            .RuleFor(c => c.Name, (k, a) => k.Name.FullName().ClampLength(min: 3, max: 50))
            .RuleFor(c => c.Email, (k, a) => k.Internet.Email(a.Name).ClampLength(min: 5))
            .RuleFor(c => c.Age, k => k.Random.Int(18, 60));

            var entities = user.Generate(100);

            context.AddRange(entities);

            context.SaveChanges();
        }
    }
}
