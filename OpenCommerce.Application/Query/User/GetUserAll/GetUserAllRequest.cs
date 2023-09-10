using OpenCommerce.Domain.DataTransferObject;
using MediatR;

namespace OpenCommerce.Application.Query.User.GetUserAll;

public sealed class GetUserAllRequest : IRequest<JsonResponse<List<GetUserAllResponse>>>
{
}