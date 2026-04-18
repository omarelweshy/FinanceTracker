using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Transactions.Queries.GetTransactions;

public class GetTransactionsHandler : IRequestHandler<GetTransactionsQuery, PagedResult<TransactionDto>>
{
    private readonly ITransactionQuery _query;

    public GetTransactionsHandler(ITransactionQuery query) => _query = query;

    public Task<PagedResult<TransactionDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken) =>
        _query.GetFilteredAsync(request.UserId, request.ToFilter());
}
