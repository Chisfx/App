using App.Application.Features.Commands;
using AutoMapper;
using App.Domain.Entities;
using App.Domain.DTOs;
namespace App.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserCommand, User>().ReverseMap();
            CreateMap<CreateUserCommand, UserModel>().ReverseMap();
            CreateMap<UpdateUserCommand, User>().ReverseMap();
            CreateMap<UpdateUserCommand, UserModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}
