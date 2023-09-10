using AutoMapper;
using OpenCommerce.Domain.Entities;

namespace OpenCommerce.Application.Query.User.GetUserAll;

public sealed class GetUserAllMapper : Profile
{
    public GetUserAllMapper()
    {
        CreateMap<UserModel, GetUserAllResponse>();
        CreateMap<GetUserAllResponse, UserModel>();
    }
}