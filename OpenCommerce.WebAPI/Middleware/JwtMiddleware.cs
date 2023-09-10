using OpenCommerce.Application.Repositories;
using OpenCommerce.Domain.Setting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace OpenCommerce.WebAPI.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtSettings _jwtSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<JwtSettings> jwtSettings)
    {
        _next = next;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task Invoke(HttpContext context, IJwtTokenHelper tokenHelper)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            await AttachUserToContext(context, token, tokenHelper);
        }

        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, string token, IJwtTokenHelper tokenHelper)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key ?? string.Empty);
            var expired = _jwtSettings.ExpireDay ?? 30;
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidIssuer = _jwtSettings.Issuer ?? string.Empty,
                ValidateAudience = false,
                ValidAudience = _jwtSettings.Audience ?? string.Empty,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Key ?? string.Empty)),
                ClockSkew = TimeSpan.FromDays(expired)
            }, out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var payload = jwtToken.Payload;
            //var userInfo = jwtToken.Claims.First(x => x.Type == "user_info").Value;
            var AuthInfo = await tokenHelper.ReadToken(payload);
            // Attach user to context on successful jwt validation
            context.Items["User"] = AuthInfo;
        }
        catch (Exception ex)
        {
            var exceptionName = ex.GetType().Name;
            var exceptionMessage = ex.Message;
            var exceptionStackTrace = ex.StackTrace;
            Console.WriteLine($"{exceptionName}{Environment.NewLine}, {exceptionMessage}{Environment.NewLine}, {exceptionStackTrace}{Environment.NewLine}");
            // Do nothing if jwt validation fails
            // User is not attached to context so request won't have access to secure routes
            // throw CustomException.TaskJwtError(Task.CurrentId);
            context.Response.StatusCode = 401;
            context.Response.ContentLength = 0;
            await context.Response.WriteAsync(string.Empty);
        }
    }
}