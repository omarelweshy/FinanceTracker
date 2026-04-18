using Dapper;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Database;

namespace FinanceTracker.Infrastructure.Repositories;

public class TransferRepository : ITransferRepository
{
    private readonly IDbSession _session;

    public TransferRepository(IDbSession session) => _session = session;

    public Task<Transfer?> GetByIdAsync(Guid id) =>
        _session.Connection.QueryFirstOrDefaultAsync<Transfer>(
            SqlLoader.Load("Transfers.GetById"), new { Id = id }, _session.Transaction);

    public Task<IEnumerable<Transfer>> GetByUserIdAsync(Guid userId) =>
        _session.Connection.QueryAsync<Transfer>(
            SqlLoader.Load("Transfers.GetByUserId"), new { UserId = userId }, _session.Transaction);

    public Task AddAsync(Transfer transfer) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("Transfers.Insert"), transfer, _session.Transaction);

    public Task DeleteAsync(Guid id) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("Transfers.Delete"), new { Id = id }, _session.Transaction);
}
