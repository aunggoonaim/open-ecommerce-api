namespace OpenCommerce.Application.Features.UserFeatures.CreateUser;

public sealed record CreateUserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
}