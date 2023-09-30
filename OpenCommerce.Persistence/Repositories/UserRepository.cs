using OpenCommerce.Application.Repositories;
using OpenCommerce.Domain.Entities;
using OpenCommerce.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace OpenCommerce.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(DataContext context) : base(context)
    {
    }
    
    public Task<User?> GetByEmail(string email, CancellationToken cancellationToken)
    {
        return Context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }
}