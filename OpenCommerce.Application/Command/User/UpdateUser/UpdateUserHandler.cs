using AutoMapper;
using OpenCommerce.Application.Repositories;
using OpenCommerce.Application.Repositories.Database;
using OpenCommerce.Domain.DataTransferObject;
using MediatR;

namespace OpenCommerce.Application.Command.User.UpdateUser;

public sealed class UpdateUserHandler : IRequestHandler<UpdateUserRequest, JsonResponse<UpdateUserResponse>>
{
    private readonly IUserService _service;
    private readonly IMapper _mapper;

    public UpdateUserHandler(IUserService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<JsonResponse<UpdateUserResponse>> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        await _service.UpdateAsync(request.id, request.form, cancellationToken);

        var mapItems = _mapper.Map<UpdateUserResponse>(request.form);

        return await Task.FromResult(new JsonResponse<UpdateUserResponse>
        {
            Data = mapItems,
            StatusCode = 200,
            Message = String.Empty,
            IsError = false
        });
    }
}