using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using OpenCommerce.Application.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using OpenCommerce.Domain.Setting;
using Microsoft.AspNetCore.Http;
using OpenCommerce.Domain.DataTransferObject.Jwt;
using OpenCommerce.Domain.DataTransferObject.Auth;
using Amazon.Runtime.Internal.Transform;

namespace OpenCommerce.Persistence.Repositories;

public class JwtTokenHelper : IJwtTokenHelper
{
    private readonly JwtSettings _jwtSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtTokenHelper(IOptions<JwtSettings> jwtSettiongs, IHttpContextAccessor httpContextAccessor)
    {
        _jwtSettings = jwtSettiongs.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public JwtTokenResult BuildToken(AuthInfoDTO? AuthModel)
    {
        if (AuthModel == null || AuthModel.username == null || AuthModel.id == null)
        {
            throw new Exception("AuthInfo must be require.");
        }

        var jwtKey = _jwtSettings.Key;
        var jwtExpires = _jwtSettings.ExpireDay;
        var jwtIssuer = _jwtSettings.Issuer;
        var jwtAudience = _jwtSettings.Audience;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? string.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTimeOffset.Now.AddDays(Convert.ToDouble(jwtExpires)).ToUnixTimeSeconds();

        var jti = Guid.NewGuid().ToString();
        var payload = new JwtPayload
            {
                {
                    "jti", jti
                },
                {
                    "id", AuthModel.id
                },
                {
                    "username", AuthModel.username
                },
                {
                    "telno", AuthModel.telno
                },
                {
                    "fullname", AuthModel.fullname
                },
                {
                    "exp", expires
                },
                {
                    "iss", jwtIssuer
                },
                {
                    "aud", jwtAudience
                },
                {
                    "address", AuthModel.address
                },
                {
                    "roleName", AuthModel.roleName
                },
                {
                    "roleCode", AuthModel.roleCode
                },
                {
                    "profileImage", AuthModel.profileImage
                },
                {
                    "startup_url", AuthModel.startupUrl
                },
                {
                    "permissions", AuthModel.permissions
                },
                {
                    "pre_register", false
                }
            };

        var header = new JwtHeader(creds);
        var secToken = new JwtSecurityToken(header, payload);
        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(secToken);
        return new JwtTokenResult(jti, expires, AuthModel, token);
    }

    public async Task<AuthInfoDTO> ReadToken(JwtPayload payload)
    {
        string payLoadToJson = payload.SerializeToJson();
        var authInfo = await Task.FromResult(JsonSerializer.Deserialize<AuthInfoDTO>(payLoadToJson) ?? new AuthInfoDTO());
        return authInfo;
    }

    public async Task<AuthInfoDTO?> GetUserFromToken()
    {
        return await Task.FromResult(_httpContextAccessor?.HttpContext?.Items["User"] as AuthInfoDTO);
    }
}
