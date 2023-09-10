using OpenCommerce.Domain.DataTransferObject;
using OpenCommerce.Domain.Entities;
using MediatR;

namespace OpenCommerce.Application.Command.User.CreateUser;

public sealed class CreateUserRequest : IRequest<JsonResponse<CreateUserResponse>>
{
    public UserModel form { get; set; } = null!;
}