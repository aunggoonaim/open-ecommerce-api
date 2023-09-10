using AutoMapper;
using OpenCommerce.Application.Repositories;
using OpenCommerce.Application.Repositories.Database;
using OpenCommerce.Domain.DataTransferObject;
using OpenCommerce.Domain.Entities;
using MediatR;

namespace OpenCommerce.Application.Command.User.CreateUser;

public sealed class CreateUserHandler : IRequestHandler<CreateUserRequest, JsonResponse<CreateUserResponse>>
{
    private readonly IUserService _userRepository;
    private readonly ISecurityService _secure;
    private readonly IJwtTokenHelper _jwt;
    private readonly IMapper _mapper;

    public CreateUserHandler(IUserService userRepository, ISecurityService secure, IJwtTokenHelper jwt, IMapper mapper)
    {
        _userRepository = userRepository;
        _secure = secure;
        _mapper = mapper;
        _jwt = jwt;
    }

    public async Task<JsonResponse<CreateUserResponse>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var tokenUser = await _jwt.GetUserFromToken();

        if (tokenUser is null)
        {
            throw new Exception("คุณไม่ได้รับอนุญาตให้เข้าถึงส่วนนี้.");
        }

        var user = _mapper.Map<UserModel>(request);

        user.roleCode = "ADM";
        user.roleName = "Administrator";
        user.createdDate = DateTime.Now;
        user.password = await _secure.SHA512(request.form.password);
        user.createdBy = tokenUser.username; 
        user.createdByName = tokenUser.fullname;

        await _userRepository.CreateAsync(user, cancellationToken);
        var UserItem = _mapper.Map<CreateUserResponse>(user);
        return new JsonResponse<CreateUserResponse>
        {
            Data = UserItem,
            IsError = false,
            StatusCode = 200
        };
    }
}