using OpenCommerce.Domain.Entities;

namespace OpenCommerce.Application.Repositories.Database;

public interface IUserService
{
    Task<List<UserModel>> GetAsync(CancellationToken token);
    Task<UserModel?> GetByIdAsync(string id, CancellationToken token);
    Task<UserModel?> GetByUsernameAndPasswordAsync(string username, string password, CancellationToken token);
    Task CreateAsync(UserModel newItem, CancellationToken token);
    Task UpdateAsync(string id, UserModel updateItem, CancellationToken token);
    Task RemoveAsync(string id, CancellationToken token);
}