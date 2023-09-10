using OpenCommerce.Domain.DataTransferObject;
using MediatR;

namespace OpenCommerce.Application.Query.User.GetUser;

public sealed record GetUserRequest(string Id) : IRequest<JsonResponse<GetUserResponse>>;