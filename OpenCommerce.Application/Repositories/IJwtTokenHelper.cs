using System.IdentityModel.Tokens.Jwt;
using OpenCommerce.Domain.DataTransferObject.Auth;
using OpenCommerce.Domain.DataTransferObject.Jwt;

namespace OpenCommerce.Application.Repositories;
public interface IJwtTokenHelper
{
    JwtTokenResult BuildToken(AuthInfoDTO? auth);
    Task<AuthInfoDTO> ReadToken(JwtPayload payload);
    Task<AuthInfoDTO?> GetUserFromToken();
}
