using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Transactions.Commands.DeleteTransaction;

public class DeleteTransactionHandler : IRequestHandler<DeleteTransactionCommand>
{
    private readonly IAccountRepository _accounts;
    private readonly ITransactionRepository _transactions;

    public DeleteTransactionHandler(IAccountRepository accounts, ITransactionRepository transactions)
    {
        _accounts = accounts;
        _transactions = transactions;
    }

    public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactions.GetByIdAsync(request.Id);
        if (transaction is null)
            throw new DomainException("Transaction not found.");

        if (transaction.Type is TransactionType.TransferIn or TransactionType.TransferOut)
            throw new DomainException("Transfer transactions must be deleted through the transfer.");

        var account = await _accounts.GetByIdForUpdateAsync(transaction.AccountId);
        if (account is null || account.UserId != request.UserId)
            throw new DomainException("Account not found.");

        if (transaction.Type == TransactionType.Income)
            account.Debit(transaction.Amount);
        else
            account.Credit(transaction.Amount);

        await _transactions.DeleteAsync(request.Id);
        await _accounts.UpdateBalanceAsync(account.Id, account.Balance);
    }
}
