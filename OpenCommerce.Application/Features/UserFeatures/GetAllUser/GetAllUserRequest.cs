using MediatR;

namespace OpenCommerce.Application.Features.UserFeatures.GetAllUser;

public sealed record GetAllUserRequest : IRequest<List<GetAllUserResponse>>;