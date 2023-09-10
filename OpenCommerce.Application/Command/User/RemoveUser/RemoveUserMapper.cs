using AutoMapper;
using OpenCommerce.Domain.Entities;

namespace OpenCommerce.Application.Command.Vehicle.RemoveUser;

public sealed class RemoveUserMapper : Profile
{
    public RemoveUserMapper()
    {
        CreateMap<UserModel, RemoveUserResponse>();
        CreateMap<RemoveUserResponse, UserModel>();
    }
}