using AutoMapper;
using OpenCommerce.Application.Repositories;
using OpenCommerce.Application.Repositories.Database;
using OpenCommerce.Domain.DataTransferObject;
using MediatR;

namespace OpenCommerce.Application.Query.User.GetUserAll;

public sealed class GetUserAllHandler : IRequestHandler<GetUserAllRequest, JsonResponse<List<GetUserAllResponse>>>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public GetUserAllHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<JsonResponse<List<GetUserAllResponse>>> Handle(GetUserAllRequest request, CancellationToken cancellationToken)
    {
        var Response = await _userService.GetAsync(cancellationToken);

        if (Response is null)
        {
            return await Task.FromResult(new JsonResponse<List<GetUserAllResponse>>
            {
                Data = null,
                StatusCode = 404,
                Message = "ไม่พบข้อมูล !",
                IsError = true
            });
        }

        var mapItems = _mapper.Map<List<GetUserAllResponse>>(Response);

        return await Task.FromResult(new JsonResponse<List<GetUserAllResponse>>
        {
            Data = mapItems,
            StatusCode = 200,
            Message = String.Empty,
            IsError = false
        });
    }
}