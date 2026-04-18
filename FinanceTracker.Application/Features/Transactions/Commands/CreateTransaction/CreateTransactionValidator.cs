using FluentValidation;

namespace FinanceTracker.Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.TransactionDate).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow.AddDays(1));
    }
}
