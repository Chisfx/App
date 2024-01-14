using App.Application.Features.Commands;
using App.Application.Mappings;
using App.Domain.DTOs;
using App.Domain.Entities;
using AutoMapper;
using System.Runtime.Serialization;
namespace App.UnitTest.Mappings
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Fact]
        public void ShouldBeValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Theory]
        [InlineData(typeof(User), typeof(CreateUserCommand))]
        [InlineData(typeof(CreateUserCommand), typeof(User))]
        [InlineData(typeof(UserModel), typeof(CreateUserCommand))]
        [InlineData(typeof(CreateUserCommand), typeof(UserModel))]
        [InlineData(typeof(UpdateUserCommand), typeof(User))]
        [InlineData(typeof(User), typeof(UpdateUserCommand))]
        [InlineData(typeof(UpdateUserCommand), typeof(UserModel))]
        [InlineData(typeof(UserModel), typeof(UpdateUserCommand))]
        [InlineData(typeof(User), typeof(UserModel))]
        [InlineData(typeof(UserModel), typeof(User))]
        public void Map_SourceToDestination_ExistConfiguration(Type origin, Type destination)
        {
            var instance = FormatterServices.GetUninitializedObject(origin);

            _mapper.Map(instance, origin, destination);
        }
    }
}
