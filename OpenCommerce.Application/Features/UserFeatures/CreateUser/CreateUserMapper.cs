using AutoMapper;
using OpenCommerce.Domain.Entities;

namespace OpenCommerce.Application.Features.UserFeatures.CreateUser;

public sealed class CreateUserMapper : Profile
{
    public CreateUserMapper()
    {
        CreateMap<CreateUserRequest, User>();
        CreateMap<User, CreateUserResponse>();
    }
}