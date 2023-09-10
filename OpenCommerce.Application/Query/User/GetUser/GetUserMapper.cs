using AutoMapper;
using OpenCommerce.Domain.Entities;

namespace OpenCommerce.Application.Query.User.GetUser;

public sealed class GetUserMapper : Profile
{
    public GetUserMapper()
    {
        CreateMap<GetUserRequest, UserModel>();
        CreateMap<UserModel, GetUserResponse>();
    }
}