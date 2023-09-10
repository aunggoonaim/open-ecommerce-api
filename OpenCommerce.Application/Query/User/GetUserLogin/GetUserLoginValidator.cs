using FluentValidation;

namespace OpenCommerce.Application.Query.User.GetUserLogin;

public sealed class GetUserLoginValidator : AbstractValidator<GetUserLoginRequest>
{
    public GetUserLoginValidator()
    {
        RuleFor(x => x.username)
            .NotEmpty()
            .MinimumLength(4);
        RuleFor(x => x.password)
            .NotEmpty()
            .MinimumLength(6);
    }
}