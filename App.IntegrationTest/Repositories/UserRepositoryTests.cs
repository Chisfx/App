using App.Application.Interfaces.Repositories;
using App.Application.Mappings;
using App.Domain.DTOs;
using App.Domain.Entities;
using App.Infrastructure.Repositories;
using App.SharedTest.DTOs;
using AutoMapper;
using Xunit.Abstractions;
namespace App.IntegrationTest.Repositories
{
    public class UserRepositoryTests : IClassFixture<SharedDatabaseFixture>
    {
        private readonly IMapper _mapper;
        private SharedDatabaseFixture Fixture { get; }
        private readonly ITestOutputHelper _testOutputHelper;
        public UserRepositoryTests(SharedDatabaseFixture fixture, ITestOutputHelper testOutputHelper)
        {
            Fixture = fixture;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
            });

            _mapper = configuration.CreateMapper();
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task GetAllUsers_ReturnsAllUsers()
        {
            using (var context = Fixture.CreateContext())
            {
                var repository = new RepositoryAsync<User>(context);

                var users = await repository.GetAllAsync();

                Assert.Equal(100, users.Count);
            }
        }

        [Fact]
        public async Task GetUserById_DoesNotExist_ReturnNull()
        {
            using (var context = Fixture.CreateContext())
            {
                var repository = new RepositoryAsync<User>(context);

                var userId = repository.Entities.Max(k => k.Id) + 1;

                var entity = await repository.GetByIdAsync(userId);

                Assert.Null(entity);
            }
        }

        [Fact]
        public async Task GetUserById_Exist_ReturnNotNull()
        {
            using (var context = Fixture.CreateContext())
            {
                var repository = new RepositoryAsync<User>(context);
                var min = repository.Entities.Min(k => k.Id);
                var max = repository.Entities.Max(k => k.Id);
                var random = new Bogus.Randomizer();
                var userId = random.Number(min, max);

                var entity = await repository.GetByIdAsync(userId);

                Assert.NotNull(entity);
            }
        }

        [Fact]
        public async Task AddUser_SavesCorrectData()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                var model = new UserFaker().Generate();
                RepositoryAsync<User> repository;
                int userId;

                using (var context = Fixture.CreateContext(transaction))
                {
                    repository = new RepositoryAsync<User>(context);

                    var entity = await repository.AddAsync(_mapper.Map<User>(model));

                    await context.SaveChangesAsync(CancellationToken.None);

                    userId = entity.Id;
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    repository = new RepositoryAsync<User>(context);

                    var entity = await repository.GetByIdAsync(userId);

                    Assert.NotNull(entity);
                    Assert.Equal(model.Name, entity.Name);
                    Assert.Equal(model.Email, entity.Email);
                    Assert.Equal(model.Age, entity.Age);
                }
            }
        }

        [Fact]
        public async Task UpdateUser_SavesCorrectData()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                var model = new UserFaker().Generate();
                RepositoryAsync<User> repository;
                int userId;
                User entity;

                using (var context = Fixture.CreateContext(transaction))
                {
                    repository = new RepositoryAsync<User>(context);
                    var min = repository.Entities.Min(k => k.Id);
                    var max = repository.Entities.Max(k => k.Id);
                    var random = new Bogus.Randomizer();
                    userId = random.Number(min, max);

                    entity = await repository.GetByIdAsync(userId);
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    repository = new RepositoryAsync<User>(context);

                    _mapper.Map(model, entity);

                    await repository.UpdateAsync(entity);

                    await context.SaveChangesAsync(CancellationToken.None);
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    repository = new RepositoryAsync<User>(context);

                    entity = await repository.GetByIdAsync(userId);

                    Assert.NotNull(entity);
                    Assert.Equal(model.Name, entity.Name);
                    Assert.Equal(model.Email, entity.Email);
                    Assert.Equal(model.Age, entity.Age);
                }
            }
        }

        [Fact]
        public async Task DeleteUser_SavesCorrectData()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                RepositoryAsync<User> repository;
                int userId;
                User entity;

                using (var context = Fixture.CreateContext(transaction))
                {
                    repository = new RepositoryAsync<User>(context);
                    var min = repository.Entities.Min(k => k.Id);
                    var max = repository.Entities.Max(k => k.Id);
                    var random = new Bogus.Randomizer();
                    userId = random.Number(min, max);

                    entity = await repository.GetByIdAsync(userId);
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    repository = new RepositoryAsync<User>(context);

                    await repository.DeleteAsync(entity);

                    await context.SaveChangesAsync(CancellationToken.None);
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    repository = new RepositoryAsync<User>(context);

                    entity = await repository.GetByIdAsync(userId);

                    Assert.Null(entity);
                }
            }
        }

    }
}
