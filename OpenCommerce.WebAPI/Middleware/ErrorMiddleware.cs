using OpenCommerce.Application.Repositories;
using OpenCommerce.Domain.Setting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace OpenCommerce.WebAPI.Middleware
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _req;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<ErrorMiddleware> _logger;

        public ErrorMiddleware(RequestDelegate req, ILogger<ErrorMiddleware> logger, IOptions<JwtSettings> jwtSettings)
        {
            _req = req;
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task Invoke(HttpContext httpContext, IJwtTokenHelper _jwt)
        {
            try
            {
                await _req(httpContext);
            }
            catch (Exception ex)
            {
                if (ex.Message != "The operation was canceled.")
                {
                    await HandleException(httpContext, _jwt, ex);
                    _logger.LogError(ex.StackTrace);
                    throw new Exception(GetMessageException(ex));
                }
            }
        }

        private async Task HandleException(HttpContext httpContext, IJwtTokenHelper jwtService, Exception ex)
        {
            CancellationToken cancellationToken = httpContext?.RequestAborted ?? CancellationToken.None;

            try
            {
                string? userId = null;
                var token = httpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var OnceDay = (DateTime.Now.Date.AddDays(1) - DateTime.Now).TotalMinutes;
                    var SpanTime = TimeSpan.FromDays(_jwtSettings.ExpireDay ?? 60);
                    tokenHandler.ValidateToken(token, new TokenValidationParameters()
                    {
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidIssuer = _jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = _jwtSettings.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Key ?? string.Empty)),
                        ClockSkew = SpanTime
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var payload = jwtToken.Payload;
                    var AuthInfo = await jwtService.ReadToken(payload);

                    userId = AuthInfo?.id;
                }

                var st = new StackTrace(ex, true);
                var frames = st.GetFrames();
                var frameMessage = string.Empty;
                if (frames is not null)
                {
                    foreach (var frame in frames)
                    {
                        var file = frame?.GetFileName();
                        var lineNo = frame?.GetFileLineNumber();
                        if (file != null && file?.IndexOf(".cs") != -1)
                        {
                            var fileName = file?.Split("\\").Last().Split("/").Last();
                            frameMessage = string.Concat("Error at file ", fileName, " in line ", lineNo, " with message ");
                            break;
                        }
                    }
                }
                // await errorLog.CreateAsync(new Domain.Entities.ErrorLogModel { errorMessage = ex.Message, innerErrorMessage = ex.InnerException?.Message, stackTrace = ex.StackTrace }, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError("Error unknow exception by {0}", e);
                // await errorLog.CreateAsync(new Domain.Entities.ErrorLogModel { errorMessage = e.Message, innerErrorMessage = e.InnerException?.Message, stackTrace = e.StackTrace }, cancellationToken);
            }
        }

        private string GetMessageException(Exception ex)
        {
            return ex.InnerException == null ? ex.Message : GetMessageException(ex.InnerException);
        }

        private string GetUserIP(HttpContext httpContext)
        {
            var remoteIpAddress = httpContext.Request.Headers["X-Forwarded-For"].ToString();
            if (string.IsNullOrEmpty(remoteIpAddress))
            {
                remoteIpAddress = httpContext.Connection?.RemoteIpAddress?.MapToIPv4().ToString();
            }
            if (remoteIpAddress == "::1")
            {
                remoteIpAddress = "localhost";
            }
            return remoteIpAddress ?? "Error";
        }
    }
}
