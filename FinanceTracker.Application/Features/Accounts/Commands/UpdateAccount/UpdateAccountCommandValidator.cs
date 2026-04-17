using FinanceTracker.Domain.Enums;
using FluentValidation;

namespace FinanceTracker.Application.Features.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Type).NotEmpty()
            .Must(t => Enum.TryParse<AccountType>(t, ignoreCase: true, out _))
            .WithMessage($"Type must be one of: {string.Join(", ", Enum.GetNames<AccountType>())}");
    }
}
