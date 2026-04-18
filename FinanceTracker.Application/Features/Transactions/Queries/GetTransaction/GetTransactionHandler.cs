using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Transactions.Queries.GetTransaction;

public class GetTransactionHandler : IRequestHandler<GetTransactionQuery, TransactionDto?>
{
    private readonly ITransactionQuery _query;

    public GetTransactionHandler(ITransactionQuery query) => _query = query;

    public Task<TransactionDto?> Handle(GetTransactionQuery request, CancellationToken cancellationToken) =>
        _query.GetByIdAsync(request.Id, request.UserId);
}
