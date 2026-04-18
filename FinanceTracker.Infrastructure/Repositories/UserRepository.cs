using Dapper;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Database;

namespace FinanceTracker.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbSession _session;

    public UserRepository(IDbSession session) => _session = session;

    public Task<User?> GetByIdAsync(Guid id) =>
        _session.Connection.QueryFirstOrDefaultAsync<User>(
            SqlLoader.Load("Users.GetById"), new { Id = id }, _session.Transaction);

    public Task<User?> GetByEmailAsync(string email) =>
        _session.Connection.QueryFirstOrDefaultAsync<User>(
            SqlLoader.Load("Users.GetByEmail"), new { Email = email }, _session.Transaction);

    public Task<bool> ExistsAsync(string email) =>
        _session.Connection.ExecuteScalarAsync<bool>(
            SqlLoader.Load("Users.Exists"), new { Email = email }, _session.Transaction);

    public Task AddAsync(User user) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("Users.Insert"), user, _session.Transaction);

    public Task UpdateAsync(User user) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("Users.Update"), user, _session.Transaction);
}
