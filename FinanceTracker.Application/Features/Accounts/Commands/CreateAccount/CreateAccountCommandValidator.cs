using FinanceTracker.Domain.Enums;
using FluentValidation;

namespace FinanceTracker.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
        RuleFor(x => x.Type).NotEmpty()
            .Must(t => Enum.TryParse<AccountType>(t, ignoreCase: true, out _))
            .WithMessage($"Type must be one of: {string.Join(", ", Enum.GetNames<AccountType>())}");
    }
}
