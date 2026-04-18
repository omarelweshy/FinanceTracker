using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.Features.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand(
    Guid UserId,
    Guid AccountId,
    Guid CategoryId,
    TransactionType Type,
    decimal Amount,
    DateTime TransactionDate,
    string? Description
) : IRequest<TransactionDto>;
