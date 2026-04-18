using FinanceTracker.Application.Common.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Transactions.Queries.GetTransaction;

public record GetTransactionQuery(Guid Id, Guid UserId) : IRequest<TransactionDto?>;
