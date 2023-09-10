using FluentValidation;

namespace OpenCommerce.Application.Command.User.CreateUser;

public sealed class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.form).NotEmpty();
    }
}