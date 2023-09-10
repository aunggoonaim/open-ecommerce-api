using AutoMapper;
using OpenCommerce.Domain.Entities;

namespace OpenCommerce.Application.Command.User.CreateUser;

public sealed class CreateUserMapper : Profile
{
    public CreateUserMapper()
    {
        CreateMap<CreateUserRequest, UserModel>();
        CreateMap<UserModel, CreateUserResponse>();
    }
}