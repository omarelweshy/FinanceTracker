using Dapper;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Database;

namespace FinanceTracker.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly IDbSession _session;

    public TransactionRepository(IDbSession session) => _session = session;

    public Task<Transaction?> GetByIdAsync(Guid id) =>
        _session.Connection.QueryFirstOrDefaultAsync<Transaction>(
            SqlLoader.Load("Transactions.GetById"), new { Id = id }, _session.Transaction);

    public Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId) =>
        _session.Connection.QueryAsync<Transaction>(
            SqlLoader.Load("Transactions.GetByAccountId"), new { AccountId = accountId }, _session.Transaction);

    public Task<IEnumerable<Transaction>> GetFilteredAsync(Guid userId, TransactionFilter filter) =>
        _session.Connection.QueryAsync<Transaction>(
            SqlLoader.Load("Transactions.GetFiltered"), new
            {
                UserId = userId,
                filter.AccountId,
                filter.CategoryId,
                Type = filter.Type?.ToString(),
                filter.From,
                filter.To,
                filter.PageSize,
                Offset = (filter.Page - 1) * filter.PageSize
            }, _session.Transaction);

    public Task AddAsync(Transaction transaction) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("Transactions.Insert"), transaction, _session.Transaction);

    public Task UpdateAsync(Transaction transaction) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("Transactions.Update"), transaction, _session.Transaction);

    public Task DeleteAsync(Guid id) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("Transactions.Delete"), new { Id = id }, _session.Transaction);
}
