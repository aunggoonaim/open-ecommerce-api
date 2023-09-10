using AutoMapper;
using OpenCommerce.Domain.Entities;

namespace OpenCommerce.Application.Command.User.UpdateUser;

public sealed class UpdateUserMapper : Profile
{
    public UpdateUserMapper()
    {
        CreateMap<UserModel, UpdateUserResponse>();
        CreateMap<UpdateUserResponse, UserModel>();
    }
}