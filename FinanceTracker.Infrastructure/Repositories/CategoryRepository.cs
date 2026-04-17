using Dapper;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Database;

namespace FinanceTracker.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CategoryRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Category>(SqlLoader.Load("Categories.GetById"), new { Id = id });
    }

    public async Task<IEnumerable<Category>> GetByUserIdAsync(Guid userId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Category>(SqlLoader.Load("Categories.GetByUserId"), new { UserId = userId });
    }

    public async Task<bool> IsReferencedByTransactionsAsync(Guid categoryId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<bool>(SqlLoader.Load("Categories.IsReferencedByTransactions"), new { CategoryId = categoryId });
    }

    public async Task AddAsync(Category category)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(SqlLoader.Load("Categories.Insert"), category);
    }

    public async Task UpdateAsync(Category category)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(SqlLoader.Load("Categories.Update"), category);
    }

    public async Task DeleteAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(SqlLoader.Load("Categories.Delete"), new { Id = id });
    }
}
