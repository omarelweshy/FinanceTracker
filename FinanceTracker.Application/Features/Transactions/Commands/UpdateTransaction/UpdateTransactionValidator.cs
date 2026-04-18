using FluentValidation;

namespace FinanceTracker.Application.Features.Transactions.Commands.UpdateTransaction;

public class UpdateTransactionValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
        RuleFor(x => x.TransactionDate).NotEmpty();
    }
}
