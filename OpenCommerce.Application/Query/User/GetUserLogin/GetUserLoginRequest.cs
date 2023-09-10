using OpenCommerce.Domain.DataTransferObject;
using MediatR;

namespace OpenCommerce.Application.Query.User.GetUserLogin;

public sealed class GetUserLoginRequest : IRequest<JsonResponse<GetUserLoginResponse>>
{
    public string? username { get; set; }
    public string? password { get; set; }
}