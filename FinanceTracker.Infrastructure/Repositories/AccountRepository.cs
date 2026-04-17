using Dapper;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Database;

namespace FinanceTracker.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AccountRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Account>(SqlLoader.Load("Accounts.GetById"), new { Id = id });
    }

    public async Task<IEnumerable<Account>> GetByUserIdAsync(Guid userId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Account>(SqlLoader.Load("Accounts.GetByUserId"), new { UserId = userId });
    }

    public async Task<bool> ExistsAsync(Guid userId, string name)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<bool>(SqlLoader.Load("Accounts.Exists"), new { UserId = userId, Name = name });
    }

    public async Task<int> CountActiveAsync(Guid userId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(SqlLoader.Load("Accounts.CountActive"), new { UserId = userId });
    }

    public async Task AddAsync(Account account)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(SqlLoader.Load("Accounts.Insert"), account);
    }

    public async Task UpdateAsync(Account account)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(SqlLoader.Load("Accounts.Update"), account);
    }

    public async Task DeleteAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(SqlLoader.Load("Accounts.Delete"), new { Id = id });
    }
}
