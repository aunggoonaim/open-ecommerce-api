using FluentValidation;

namespace OpenCommerce.Application.Command.User.UpdateUser;

public sealed class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserValidator()
    {
    }
}