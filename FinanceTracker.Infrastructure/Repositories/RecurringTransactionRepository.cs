using Dapper;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Database;

namespace FinanceTracker.Infrastructure.Repositories;

public class RecurringTransactionRepository : IRecurringTransactionRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public RecurringTransactionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<RecurringTransaction?> GetByIdAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<RecurringTransaction>(SqlLoader.Load("RecurringTransactions.GetById"), new { Id = id });
    }

    public async Task<IEnumerable<RecurringTransaction>> GetByUserIdAsync(Guid userId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<RecurringTransaction>(SqlLoader.Load("RecurringTransactions.GetByUserId"), new { UserId = userId });
    }

    public async Task<IEnumerable<RecurringTransaction>> GetPendingAsync(DateTime asOf)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<RecurringTransaction>(SqlLoader.Load("RecurringTransactions.GetPending"), new { AsOf = asOf });
    }

    public async Task AddAsync(RecurringTransaction recurringTransaction)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(SqlLoader.Load("RecurringTransactions.Insert"), recurringTransaction);
    }

    public async Task UpdateAsync(RecurringTransaction recurringTransaction)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(SqlLoader.Load("RecurringTransactions.Update"), recurringTransaction);
    }

    public async Task DeleteAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(SqlLoader.Load("RecurringTransactions.Delete"), new { Id = id });
    }
}
