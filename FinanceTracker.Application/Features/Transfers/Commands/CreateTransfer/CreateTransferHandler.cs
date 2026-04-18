using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Transfers.Commands.CreateTransfer;

public class CreateTransferHandler : IRequestHandler<CreateTransferCommand, TransferDto>
{
    private readonly IDbSession _session;
    private readonly IAccountRepository _accounts;
    private readonly ITransactionRepository _transactions;
    private readonly ITransferRepository _transfers;
    private readonly ITransferQuery _query;

    public CreateTransferHandler(IDbSession session, IAccountRepository accounts,
        ITransactionRepository transactions, ITransferRepository transfers, ITransferQuery query)
    {
        _session = session;
        _accounts = accounts;
        _transactions = transactions;
        _transfers = transfers;
        _query = query;
    }

    public async Task<TransferDto> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
    {
        await _session.BeginTransactionAsync();
        try
        {
            // Lock both accounts in a consistent order (by ID) to prevent deadlocks
            var (firstId, secondId) = request.FromAccountId < request.ToAccountId
                ? (request.FromAccountId, request.ToAccountId)
                : (request.ToAccountId, request.FromAccountId);

            var first = await _accounts.GetByIdForUpdateAsync(firstId);
            var second = await _accounts.GetByIdForUpdateAsync(secondId);

            var fromAccount = first?.Id == request.FromAccountId ? first : second;
            var toAccount = first?.Id == request.ToAccountId ? first : second;

            if (fromAccount is null || fromAccount.UserId != request.UserId)
                throw new DomainException("Source account not found.");
            if (toAccount is null || toAccount.UserId != request.UserId)
                throw new DomainException("Destination account not found.");

            // Domain rules enforce balance constraints
            fromAccount.Debit(request.Amount);
            toAccount.Credit(request.Amount);

            var debitTx = Transaction.Create(fromAccount.Id, null, TransactionType.TransferOut,
                request.Amount, request.TransferDate, request.Note);
            var creditTx = Transaction.Create(toAccount.Id, null, TransactionType.TransferIn,
                request.Amount, request.TransferDate, request.Note);

            await _transactions.AddAsync(debitTx);
            await _transactions.AddAsync(creditTx);
            await _accounts.UpdateBalanceAsync(fromAccount.Id, fromAccount.Balance);
            await _accounts.UpdateBalanceAsync(toAccount.Id, toAccount.Balance);

            var transfer = Transfer.Create(debitTx.Id, creditTx.Id, request.Amount, request.Note);
            await _transfers.AddAsync(transfer);

            await _session.CommitAsync();

            return (await _query.GetByIdAsync(transfer.Id, request.UserId))!;
        }
        catch
        {
            await _session.RollbackAsync();
            throw;
        }
    }
}
