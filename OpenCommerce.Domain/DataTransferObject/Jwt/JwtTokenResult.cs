using System.Text.Json.Serialization;
using OpenCommerce.Domain.DataTransferObject.Auth;

namespace OpenCommerce.Domain.DataTransferObject.Jwt
{
    public class JwtTokenResult
    {
        public JwtTokenResult()
        {
        }

        public JwtTokenResult(string jti = "", long expires = 0, AuthInfoDTO? authInfo = null, string token = "")
        {
            this.jti = jti;
            this.expires = expires;
            this.authInfo = authInfo;
            this.token = token;
        }
        [JsonIgnore]
        public string jti { get; } = null!;
        [JsonIgnore]
        public long expires { get; }
        [JsonIgnore]
        public AuthInfoDTO? authInfo { get; }
        public string token { get; } = null!;
        [JsonIgnore]
        public string? message { get; set; }
    }
}