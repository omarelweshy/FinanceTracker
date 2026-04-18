using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Transactions.Queries.GetTransactions;

public record GetTransactionsQuery(
    Guid UserId,
    Guid? AccountId,
    Guid? CategoryId,
    TransactionType? Type,
    DateTime? From,
    DateTime? To,
    int Page = 1,
    int PageSize = 20
) : IRequest<PagedResult<TransactionDto>>
{
    public TransactionFilter ToFilter() => new()
    {
        AccountId = AccountId,
        CategoryId = CategoryId,
        Type = Type,
        From = From,
        To = To,
        Page = Page,
        PageSize = PageSize
    };
}
