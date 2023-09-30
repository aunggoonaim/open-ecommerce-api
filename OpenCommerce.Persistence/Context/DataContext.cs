using OpenCommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace OpenCommerce.Persistence.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
}