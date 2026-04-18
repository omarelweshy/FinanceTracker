using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Events;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    private readonly IDbSession _session;
    private readonly IAccountRepository _accounts;
    private readonly ITransactionRepository _transactions;
    private readonly ITransactionQuery _query;
    private readonly IEventBus _eventBus;

    public CreateTransactionHandler(IDbSession session, IAccountRepository accounts,
        ITransactionRepository transactions, ITransactionQuery query, IEventBus eventBus)
    {
        _session = session;
        _accounts = accounts;
        _transactions = transactions;
        _query = query;
        _eventBus = eventBus;
    }

    public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        await _session.BeginTransactionAsync();
        try
        {
            var account = await _accounts.GetByIdForUpdateAsync(request.AccountId);
            if (account is null || account.UserId != request.UserId)
                throw new DomainException("Account not found.");

            if (request.Type == TransactionType.Income || request.Type == TransactionType.TransferIn)
                account.Credit(request.Amount);
            else
                account.Debit(request.Amount);

            var transaction = Transaction.Create(
                request.AccountId, request.CategoryId, request.Type,
                request.Amount, request.TransactionDate, request.Description);

            await _transactions.AddAsync(transaction);
            await _accounts.UpdateBalanceAsync(account.Id, account.Balance);
            await _session.CommitAsync();

            await _eventBus.PublishAsync(new TransactionCreatedEvent(
                transaction.Id, request.UserId, request.AccountId,
                request.CategoryId, request.Type.ToString(),
                request.Amount, request.TransactionDate), cancellationToken);

            return (await _query.GetByIdAsync(transaction.Id, request.UserId))!;
        }
        catch
        {
            await _session.RollbackAsync();
            throw;
        }
    }
}
