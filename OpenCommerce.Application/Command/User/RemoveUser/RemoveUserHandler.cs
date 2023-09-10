using AutoMapper;
using OpenCommerce.Application.Repositories;
using OpenCommerce.Application.Repositories.Database;
using OpenCommerce.Domain.DataTransferObject;
using MediatR;

namespace OpenCommerce.Application.Command.Vehicle.RemoveUser;

public sealed class RemoveUserHandler : IRequestHandler<RemoveUserRequest, JsonResponse<RemoveUserResponse>>
{
    private readonly IUserService _service;
    private readonly IMapper _mapper;

    public RemoveUserHandler(IUserService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    
    public async Task<JsonResponse<RemoveUserResponse>> Handle(RemoveUserRequest request, CancellationToken cancellationToken)
    {
        await _service.RemoveAsync(request.id, cancellationToken);
        return await Task.FromResult(new JsonResponse<RemoveUserResponse>
        {
            Data = null,
            StatusCode = 200,
            Message = String.Empty,
            IsError = false
        });
    }
}