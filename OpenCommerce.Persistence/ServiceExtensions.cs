using OpenCommerce.Application.Repositories;
using OpenCommerce.Persistence.Repositories;
using OpenCommerce.Application.Repositories.Database;
using OpenCommerce.Persistence.Repositories.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OpenCommerce.Persistence;

public static class ServiceExtensions
{
    public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddHttpContextAccessor();

        services.AddScoped<IUserService, UserService>();

        services.AddScoped<ICryptoManagerService, CryptoManagerService>();
        services.AddScoped<IExcelService, ExcelService>();
        services.AddScoped<IFileService, FileService>();
        
        services.AddScoped<IMemCacheService, MemCacheService>();
        services.AddScoped<INetworkService, NetworkService>();
        services.AddScoped<ISecurityService, SecurityService>();

        services.AddSingleton<IJwtTokenHelper, JwtTokenHelper>();

        services.AddHttpClient("line", c =>
        {
            c.BaseAddress = new Uri("https://notify-api.line.me");
        });
    }
}