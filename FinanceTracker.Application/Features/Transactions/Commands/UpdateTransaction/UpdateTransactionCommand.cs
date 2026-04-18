using FinanceTracker.Application.Common.Behaviors;
using FinanceTracker.Application.Common.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Transactions.Commands.UpdateTransaction;

public record UpdateTransactionCommand(
    Guid Id,
    Guid UserId,
    decimal Amount,
    Guid? CategoryId,
    DateTime TransactionDate,
    string? Description
) : IRequest<TransactionDto>, ITransactionalRequest;
