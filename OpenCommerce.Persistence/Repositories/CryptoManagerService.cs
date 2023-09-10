using OpenCommerce.Application.Repositories;
using Dapper;

namespace OpenCommerce.Persistence.Repositories;

public class CryptoManagerService : ICryptoManagerService
{
    public async Task<string?> SHA512(string input)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        using (var hash = System.Security.Cryptography.SHA512.Create())
        {
            var hashedInputBytes = hash.ComputeHash(bytes);
            var hashedInputStringBuilder = new System.Text.StringBuilder(128);
            foreach (var b in hashedInputBytes)
            {
                hashedInputStringBuilder.Append(b.ToString("X2"));
            }
            return await Task.FromResult(hashedInputStringBuilder?.ToString());
        }
    }

    public async Task<string?> SHA3_512(string input)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hash = new Org.BouncyCastle.Crypto.Digests.Sha3Digest(512);

        hash.BlockUpdate(bytes, 0, bytes.Length);

        byte[] result = new byte[64];
        hash.DoFinal(result, 0);

        return await Task.FromResult(BitConverter.ToString(result).Replace("-", "").ToLowerInvariant());
    }
}
