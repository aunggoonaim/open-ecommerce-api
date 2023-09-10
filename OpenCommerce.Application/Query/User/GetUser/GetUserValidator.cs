using FluentValidation;

namespace OpenCommerce.Application.Query.User.GetUser;

public sealed class GetUserValidator : AbstractValidator<GetUserRequest>
{
    public GetUserValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}