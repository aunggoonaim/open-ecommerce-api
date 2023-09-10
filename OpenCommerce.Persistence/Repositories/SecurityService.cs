using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using OpenCommerce.Application.Repositories;
using OpenCommerce.Domain.Setting;

namespace OpenCommerce.Persistence.Repositories;

public class SecurityService : ISecurityService
{
    private readonly AppSetting _option;

    public SecurityService(IOptions<AppSetting> option)
    {
        _option = option.Value;
    }

    public async Task<string> SHA512(string input)
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
            return await Task.FromResult(hashedInputStringBuilder?.ToString() ?? string.Empty);
        }
    }

    public async Task<string?> AES256EncryptToString(string? input)
    {
        var Key = System.Text.Encoding.ASCII.GetBytes(_option.AesKey);
        var IV = System.Text.Encoding.ASCII.GetBytes(_option.AesIV);

        try
        {
            if (input == null || input.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return await Task.FromResult(Convert.ToBase64String(encrypted));
        }
        catch (Exception)
        {
            // TODO: Log the error 
            return null;
        }
    }

    public async Task<string?> AES256DecryptToString(string? input)
    {
        var Key = System.Text.Encoding.ASCII.GetBytes(_option.AesKey);
        var IV = System.Text.Encoding.ASCII.GetBytes(_option.AesIV);

        try
        {
            if (input == null || input.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string? plaintext = null;
            byte[] cipherText = Convert.FromBase64String(input.Replace(' ', '+'));

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return await Task.FromResult(plaintext);
        }
        catch (Exception)
        {
            // TODO: Log the error 
            return null;
        }
    }
}
