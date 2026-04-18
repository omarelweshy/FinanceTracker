using Dapper;
using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Database;

namespace FinanceTracker.Infrastructure.Repositories.Queries;

public class TransactionQueryRepository : ITransactionQuery
{
    private readonly IDbSession _session;

    public TransactionQueryRepository(IDbSession session) => _session = session;

    public async Task<TransactionDto?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _session.Connection.QueryFirstOrDefaultAsync<TransactionDto>(
            SqlLoader.Load("Transactions.GetByIdWithDetails"),
            new { Id = id, UserId = userId },
            _session.Transaction);
    }

    public async Task<PagedResult<TransactionDto>> GetFilteredAsync(Guid userId, TransactionFilter filter)
    {
        var param = new
        {
            UserId = userId,
            filter.AccountId,
            filter.CategoryId,
            Type = filter.Type?.ToString(),
            filter.From,
            filter.To,
            filter.PageSize,
            Offset = (filter.Page - 1) * filter.PageSize
        };

        using var multi = await _session.Connection.QueryMultipleAsync(
            SqlLoader.Load("Transactions.GetFilteredWithCount"), param, _session.Transaction);

        var total = await multi.ReadFirstAsync<int>();
        var items = await multi.ReadAsync<TransactionDto>();

        return new PagedResult<TransactionDto>(items, total, filter.Page, filter.PageSize);
    }
}
