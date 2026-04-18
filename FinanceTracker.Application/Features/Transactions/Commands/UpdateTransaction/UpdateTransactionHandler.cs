using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Transactions.Commands.UpdateTransaction;

public class UpdateTransactionHandler : IRequestHandler<UpdateTransactionCommand, TransactionDto>
{
    private readonly IDbSession _session;
    private readonly IAccountRepository _accounts;
    private readonly ITransactionRepository _transactions;
    private readonly ITransactionQuery _query;

    public UpdateTransactionHandler(IDbSession session, IAccountRepository accounts,
        ITransactionRepository transactions, ITransactionQuery query)
    {
        _session = session;
        _accounts = accounts;
        _transactions = transactions;
        _query = query;
    }

    public async Task<TransactionDto> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        await _session.BeginTransactionAsync();
        try
        {
            var transaction = await _transactions.GetByIdAsync(request.Id);
            if (transaction is null)
                throw new DomainException("Transaction not found.");

            if (transaction.Type is TransactionType.TransferIn or TransactionType.TransferOut)
                throw new DomainException("Transfer transactions cannot be updated independently.");

            var account = await _accounts.GetByIdForUpdateAsync(transaction.AccountId);
            if (account is null || account.UserId != request.UserId)
                throw new DomainException("Account not found.");

            // Reverse the old effect
            if (transaction.Type == TransactionType.Income)
                account.Debit(transaction.Amount);
            else
                account.Credit(transaction.Amount);

            // Apply the new effect
            if (transaction.Type == TransactionType.Income)
                account.Credit(request.Amount);
            else
                account.Debit(request.Amount);

            transaction.Update(request.Amount, request.CategoryId, request.TransactionDate, request.Description);

            await _transactions.UpdateAsync(transaction);
            await _accounts.UpdateBalanceAsync(account.Id, account.Balance);
            await _session.CommitAsync();

            return (await _query.GetByIdAsync(transaction.Id, request.UserId))!;
        }
        catch
        {
            await _session.RollbackAsync();
            throw;
        }
    }
}
