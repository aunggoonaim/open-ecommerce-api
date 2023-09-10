namespace OpenCommerce.Application.Repositories;

public interface INetworkService
{
    string GetUserIP();
    string GetUrlAbsolutePath();
}
