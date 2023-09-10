using Microsoft.AspNetCore.Http.Extensions;
using OpenCommerce.Application.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace OpenCommerce.Persistence.Repositories;

public class NetworkService : INetworkService
{
    private readonly ILogger<NetworkService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NetworkService(ILogger<NetworkService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserIP()
    {
        var remoteIpAddress = _httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].ToString();
        if (string.IsNullOrEmpty(remoteIpAddress))
        {
            remoteIpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString();
        }

        if (remoteIpAddress == "::1")
        {
            remoteIpAddress = "localhost";
        }
        return remoteIpAddress ?? "Error";
    }
    
    public string GetUrlAbsolutePath()
    {
        return _httpContextAccessor.HttpContext?.Request?.GetDisplayUrl() ?? "";
    }
}
