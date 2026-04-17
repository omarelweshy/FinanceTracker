using Dapper;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Database;

namespace FinanceTracker.Infrastructure.Repositories;

public class BudgetRepository : IBudgetRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public BudgetRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Budget?> GetByIdAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Budget>(SqlLoader.Load("Budgets.GetById"), new { Id = id });
    }

    public async Task<IEnumerable<Budget>> GetByUserIdAsync(Guid userId, int month, int year)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Budget>(SqlLoader.Load("Budgets.GetByUserId"), new { UserId = userId, Month = month, Year = year });
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid categoryId, int month, int year)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<bool>(SqlLoader.Load("Budgets.Exists"), new { UserId = userId, CategoryId = categoryId, Month = month, Year = year });
    }

    public async Task<decimal> GetSpentAmountAsync(Guid userId, Guid categoryId, int month, int year)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<decimal>(SqlLoader.Load("Budgets.GetSpentAmount"), new { UserId = userId, CategoryId = categoryId, Month = month, Year = year });
    }

    public async Task AddAsync(Budget budget)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(SqlLoader.Load("Budgets.Insert"), budget);
    }

    public async Task UpdateAsync(Budget budget)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(SqlLoader.Load("Budgets.Update"), budget);
    }

    public async Task DeleteAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(SqlLoader.Load("Budgets.Delete"), new { Id = id });
    }
}
