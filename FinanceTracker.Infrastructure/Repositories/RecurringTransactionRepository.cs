using Dapper;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Database;

namespace FinanceTracker.Infrastructure.Repositories;

public class RecurringTransactionRepository : IRecurringTransactionRepository
{
    private readonly IDbSession _session;

    public RecurringTransactionRepository(IDbSession session) => _session = session;

    public Task<RecurringTransaction?> GetByIdAsync(Guid id) =>
        _session.Connection.QueryFirstOrDefaultAsync<RecurringTransaction>(
            SqlLoader.Load("RecurringTransactions.GetById"), new { Id = id }, _session.Transaction);

    public Task<IEnumerable<RecurringTransaction>> GetByUserIdAsync(Guid userId) =>
        _session.Connection.QueryAsync<RecurringTransaction>(
            SqlLoader.Load("RecurringTransactions.GetByUserId"), new { UserId = userId }, _session.Transaction);

    public Task<IEnumerable<RecurringTransaction>> GetPendingAsync(DateTime asOf) =>
        _session.Connection.QueryAsync<RecurringTransaction>(
            SqlLoader.Load("RecurringTransactions.GetPending"), new { AsOf = asOf }, _session.Transaction);

    public Task AddAsync(RecurringTransaction recurringTransaction) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("RecurringTransactions.Insert"), recurringTransaction, _session.Transaction);

    public Task UpdateAsync(RecurringTransaction recurringTransaction) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("RecurringTransactions.Update"), recurringTransaction, _session.Transaction);

    public Task DeleteAsync(Guid id) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("RecurringTransactions.Delete"), new { Id = id }, _session.Transaction);
}
