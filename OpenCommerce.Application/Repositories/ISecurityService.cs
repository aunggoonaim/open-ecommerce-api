namespace OpenCommerce.Application.Repositories;

public interface ISecurityService
{
    Task<string> SHA512(string input);
    Task<string?> AES256EncryptToString(string? input);
    Task<string?> AES256DecryptToString(string? input);
}
