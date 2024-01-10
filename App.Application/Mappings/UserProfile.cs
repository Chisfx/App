using App.Application.Features.Commands;
using AutoMapper;
using App.Domain.Entities;
using App.Domain.DTOs;
namespace App.Application.Mappings
{
    /// <summary>
    /// Represents a mapping profile for the User entity.
    /// </summary>
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Maps CreateUserCommand to User and vice versa
            CreateMap<CreateUserCommand, User>().ReverseMap();

            // Maps CreateUserCommand to UserModel and vice versa
            CreateMap<CreateUserCommand, UserModel>().ReverseMap();

            // Maps UpdateUserCommand to User and vice versa
            CreateMap<UpdateUserCommand, User>().ReverseMap();

            // Maps UpdateUserCommand to UserModel and vice versa
            CreateMap<UpdateUserCommand, UserModel>().ReverseMap();

            // Maps User to UserModel and vice versa
            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}
