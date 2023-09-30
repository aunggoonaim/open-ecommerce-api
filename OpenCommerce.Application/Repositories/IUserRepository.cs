using OpenCommerce.Domain.Entities;

namespace OpenCommerce.Application.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmail(string email, CancellationToken cancellationToken);
}