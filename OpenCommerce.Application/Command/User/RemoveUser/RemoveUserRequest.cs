using OpenCommerce.Domain.DataTransferObject;
using MediatR;

namespace OpenCommerce.Application.Command.Vehicle.RemoveUser;

public sealed class RemoveUserRequest : IRequest<JsonResponse<RemoveUserResponse>>
{
    public string id { get; set; } = null!;
}