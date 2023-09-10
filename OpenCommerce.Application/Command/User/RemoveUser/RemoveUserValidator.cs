using FluentValidation;

namespace OpenCommerce.Application.Command.Vehicle.RemoveUser;

public sealed class RemoveUserValidator : AbstractValidator<RemoveUserRequest>
{
    public RemoveUserValidator()
    {
    }
}