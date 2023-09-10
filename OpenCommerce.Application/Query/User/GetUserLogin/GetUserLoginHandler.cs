using OpenCommerce.Application.Repositories;
using OpenCommerce.Application.Repositories.Database;
using OpenCommerce.Domain.DataTransferObject;
using OpenCommerce.Domain.DataTransferObject.Auth;
using OpenCommerce.Domain.DataTransferObject.Cache;
using OpenCommerce.Domain.DataTransferObject.Jwt;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace OpenCommerce.Application.Query.User.GetUserLogin;

public sealed class GetUserHandler : IRequestHandler<GetUserLoginRequest, JsonResponse<GetUserLoginResponse>>
{
    private readonly IUserService _userRepository;
    private readonly INetworkService _networkService;
    private readonly ISecurityService _securityService;
    private readonly IJwtTokenHelper _jwtTokenHelper;
    private readonly IMemoryCache _memCache;

    public GetUserHandler(
        IUserService userRepository,
        INetworkService networkService,
        ISecurityService securityService,
        IJwtTokenHelper jwtTokenHelper,
        IMemoryCache memCache)
    {
        _userRepository = userRepository;
        _networkService = networkService;
        _securityService = securityService;
        _jwtTokenHelper = jwtTokenHelper;
        _memCache = memCache;
    }

    public async Task<JsonResponse<GetUserLoginResponse>> Handle(GetUserLoginRequest request, CancellationToken cancellationToken)
    {
        var jwt = new JwtTokenResult();
        var resultUserDTO = new AuthInfoDTO();
        var Ipaddress = _networkService.GetUserIP();

        if (string.IsNullOrEmpty(request.username) || string.IsNullOrEmpty(request.password))
        {
            return await Task.FromResult(new JsonResponse<GetUserLoginResponse>
            {
                Data = null,
                StatusCode = 400,
                Message = "UserId และ Password ห้ามเว้นว่าง !",
                IsError = true
            });
        }

        var HashPassword = await _securityService.SHA512(request.password);

        var VerifyUserAttemp = ValidateUserAttempLoginCount(_memCache, request.username, Ipaddress);
        if (VerifyUserAttemp is not null && VerifyUserAttemp.count >= 3)
        {
            var TimeStamp = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
            return await Task.FromResult(new JsonResponse<GetUserLoginResponse>
            {
                Data = null,
                StatusCode = 500,
                Message = $"ผิดพลาด ! คุณได้ใส่รหัสผ่านผิดพลาด 3 ครั้งติดต่อกัน กรุณาลองใหม่ในอีก 1 นาทีค่ะ ( {TimeStamp} เวลาประเทศไทย )",
                IsError = true
            });
        }

        var UserItem = await _userRepository.GetByUsernameAndPasswordAsync(request.username, HashPassword, cancellationToken);

        if (UserItem is null)
        {
            return await Task.FromResult(new JsonResponse<GetUserLoginResponse>
            {
                Data = null,
                StatusCode = 404,
                Message = "ชื่อผู้ใช้งานหรือรหัสผ่านผิดพลาด !",
                IsError = true
            });
        }

        if (UserItem.password != HashPassword)
        {
            return await Task.FromResult(new JsonResponse<GetUserLoginResponse>
            {
                Data = null,
                StatusCode = 404,
                Message = $"รหัสผ่านผิดพลาดครั้งที่ {VerifyUserAttemp?.count ?? 1}/3 !",
                IsError = true
            });
        }

        if (UserItem == null)
        {
            return await Task.FromResult(new JsonResponse<GetUserLoginResponse>
            {
                Data = null,
                StatusCode = 404,
                Message = "ชื่อผู้ใช้งานหรือรหัสผ่านผิดพลาด !",
                IsError = true
            });
        }

        resultUserDTO.id = UserItem.id;
        resultUserDTO.profileImage = UserItem.profileImage;
        resultUserDTO.username = UserItem.username;
        resultUserDTO.fullname = UserItem.fullName;
        resultUserDTO.roleName = UserItem.roleName;
        resultUserDTO.roleCode = UserItem.roleCode;
        resultUserDTO.address = UserItem.address;
        resultUserDTO.startupUrl = "/dashboard";

        if (cancellationToken.IsCancellationRequested)
        {
            throw new Exception("Task has been canceled.");
        }

        UserItem.lastLoginDate = DateTime.Now;

        var jwtTokenResult = _jwtTokenHelper.BuildToken(resultUserDTO);

        ClearAttempLoginCount(_memCache, request.username, Ipaddress);

        return await Task.FromResult(new JsonResponse<GetUserLoginResponse>
        {
            Data = new GetUserLoginResponse { token = jwtTokenResult.token },
            StatusCode = 200,
            Message = String.Empty,
            IsError = false
        });
    }

    private AttempLoginTrans? ValidateUserAttempLoginCount(IMemoryCache _memCache, string email, string ipaddress)
    {
        AttempLoginTrans? cache;
        var CacheUpdateKey = $"AttempLoginTrans_{email}_{ipaddress}";
        if (!_memCache.TryGetValue(CacheUpdateKey, out cache))
        {
            if (cache == null)
            {
                cache = new AttempLoginTrans();
            }
        }
        if (cache is not null)
        {
            cache.email = email;
            cache.ipaddress = ipaddress;
            cache.count += 1;
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(1));
            _memCache.Set(CacheUpdateKey, cache, cacheEntryOptions);
        }
        return cache;
    }

    private void ClearAttempLoginCount(IMemoryCache _memCache, string email, string ipaddress)
    {
        var CacheUpdateKey = $"AttempLoginTrans_{email}_{ipaddress}";
        _memCache.Remove(CacheUpdateKey);
    }
}