using OpenCommerce.Domain.Common;

namespace OpenCommerce.Domain.Entities;

public sealed class User : BaseEntity
{
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
}