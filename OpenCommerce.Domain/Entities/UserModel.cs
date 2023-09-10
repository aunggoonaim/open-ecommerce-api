using OpenCommerce.Domain.Common;

namespace OpenCommerce.Domain.Entities;

public class UserModel : BaseEntity
{
    public string username { get; set; } = null!;
    public string password { get; set; } = null!;
    public string fullName { get; set; } = null!;
    public string? address { get; set; }
    public string? telno { get; set; }
    public string roleCode { get; set; } = null!;
    public string roleName { get; set; } = null!;
    public string? profileImage { get; set; }
    public DateTime? lastLoginDate { get; set; }
}