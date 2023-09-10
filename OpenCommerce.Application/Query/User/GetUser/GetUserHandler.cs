using AutoMapper;
using OpenCommerce.Application.Repositories;
using OpenCommerce.Application.Repositories.Database;
using OpenCommerce.Domain.DataTransferObject;
using MediatR;

namespace OpenCommerce.Application.Query.User.GetUser;

public sealed class GetUserHandler : IRequestHandler<GetUserRequest, JsonResponse<GetUserResponse>>
{
    private readonly IUserService _userRepository;
    private readonly IMapper _mapper;

    public GetUserHandler(IUserService userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<JsonResponse<GetUserResponse>> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        var response = _mapper.Map<GetUserResponse>(user);
        return new JsonResponse<GetUserResponse>
        {
            Data = response,
            IsError = false,
            StatusCode = 200
        };
    }
}