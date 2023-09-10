namespace OpenCommerce.Application.Repositories;

public interface ICryptoManagerService
{
    Task<string?> SHA512(string input);
    Task<string?> SHA3_512(string input);
}
