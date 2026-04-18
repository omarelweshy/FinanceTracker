using Dapper;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Database;

namespace FinanceTracker.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IDbSession _session;

    public AccountRepository(IDbSession session) => _session = session;

    public Task<Account?> GetByIdAsync(Guid id) =>
        _session.Connection.QueryFirstOrDefaultAsync<Account>(
            SqlLoader.Load("Accounts.GetById"), new { Id = id }, _session.Transaction);

    public Task<Account?> GetByIdForUpdateAsync(Guid id) =>
        _session.Connection.QueryFirstOrDefaultAsync<Account>(
            SqlLoader.Load("Accounts.GetByIdForUpdate"), new { Id = id }, _session.Transaction);

    public Task<IEnumerable<Account>> GetByUserIdAsync(Guid userId) =>
        _session.Connection.QueryAsync<Account>(
            SqlLoader.Load("Accounts.GetByUserId"), new { UserId = userId }, _session.Transaction);

    public Task<bool> ExistsAsync(Guid userId, string name) =>
        _session.Connection.ExecuteScalarAsync<bool>(
            SqlLoader.Load("Accounts.Exists"), new { UserId = userId, Name = name }, _session.Transaction);

    public Task<int> CountActiveAsync(Guid userId) =>
        _session.Connection.ExecuteScalarAsync<int>(
            SqlLoader.Load("Accounts.CountActive"), new { UserId = userId }, _session.Transaction);

    public Task AddAsync(Account account) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("Accounts.Insert"), account, _session.Transaction);

    public Task UpdateAsync(Account account) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("Accounts.Update"), account, _session.Transaction);

    public Task UpdateBalanceAsync(Guid id, decimal balance) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("Accounts.UpdateBalance"), new { Id = id, Balance = balance }, _session.Transaction);

    public Task DeleteAsync(Guid id) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("Accounts.Delete"), new { Id = id }, _session.Transaction);
}
