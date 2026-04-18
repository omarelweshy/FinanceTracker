using Dapper;
using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Infrastructure.Database;

namespace FinanceTracker.Infrastructure.Repositories.Queries;

public class TransferQueryRepository : ITransferQuery
{
    private readonly IDbSession _session;

    public TransferQueryRepository(IDbSession session) => _session = session;

    public async Task<TransferDto?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _session.Connection.QueryFirstOrDefaultAsync<TransferDto>(
            SqlLoader.Load("Transfers.GetByIdWithDetails"),
            new { Id = id, UserId = userId },
            _session.Transaction);
    }

    public async Task<PagedResult<TransferDto>> GetByUserIdAsync(Guid userId, int page, int pageSize)
    {
        var param = new { UserId = userId, PageSize = pageSize, Offset = (page - 1) * pageSize };

        using var multi = await _session.Connection.QueryMultipleAsync(
            SqlLoader.Load("Transfers.GetByUserIdWithDetails"), param, _session.Transaction);

        var items = await multi.ReadAsync<TransferDto>();
        var total = await multi.ReadFirstAsync<int>();

        return new PagedResult<TransferDto>(items, total, page, pageSize);
    }
}
