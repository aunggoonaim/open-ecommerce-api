using OpenCommerce.Domain.DataTransferObject;
using OpenCommerce.Domain.Entities;
using MediatR;

namespace OpenCommerce.Application.Command.User.UpdateUser;

public sealed class UpdateUserRequest : IRequest<JsonResponse<UpdateUserResponse>>
{
    public string id { get; set; } = null!;
    public UserModel form { get; set; } = null!;
}