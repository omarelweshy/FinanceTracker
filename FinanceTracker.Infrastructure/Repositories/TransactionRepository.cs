using Dapper;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Database;

namespace FinanceTracker.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TransactionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Transaction>(SqlLoader.Load("Transactions.GetById"), new { Id = id });
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Transaction>(SqlLoader.Load("Transactions.GetByAccountId"), new { AccountId = accountId });
    }

    public async Task<IEnumerable<Transaction>> GetFilteredAsync(Guid userId, TransactionFilter filter)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Transaction>(SqlLoader.Load("Transactions.GetFiltered"), new
        {
            UserId = userId,
            filter.AccountId,
            filter.CategoryId,
            Type = filter.Type?.ToString(),
            filter.From,
            filter.To,
            filter.PageSize,
            Offset = (filter.Page - 1) * filter.PageSize
        });
    }

    public async Task AddAsync(Transaction transaction)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(SqlLoader.Load("Transactions.Insert"), transaction);
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(SqlLoader.Load("Transactions.Update"), transaction);
    }

    public async Task DeleteAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(SqlLoader.Load("Transactions.Delete"), new { Id = id });
    }
}
