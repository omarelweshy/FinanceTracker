using FluentValidation;

namespace FinanceTracker.Application.Features.Transfers.Commands.CreateTransfer;

public class CreateTransferValidator : AbstractValidator<CreateTransferCommand>
{
    public CreateTransferValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
        RuleFor(x => x.FromAccountId).NotEmpty();
        RuleFor(x => x.ToAccountId).NotEmpty();
        RuleFor(x => x.TransferDate).NotEmpty();
        RuleFor(x => x).Must(x => x.FromAccountId != x.ToAccountId)
            .WithMessage("Source and destination accounts must be different.");
    }
}
