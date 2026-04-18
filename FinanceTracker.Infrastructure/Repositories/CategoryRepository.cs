using Dapper;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Database;

namespace FinanceTracker.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IDbSession _session;

    public CategoryRepository(IDbSession session) => _session = session;

    public Task<Category?> GetByIdAsync(Guid id) =>
        _session.Connection.QueryFirstOrDefaultAsync<Category>(
            SqlLoader.Load("Categories.GetById"), new { Id = id }, _session.Transaction);

    public Task<IEnumerable<Category>> GetByUserIdAsync(Guid userId) =>
        _session.Connection.QueryAsync<Category>(
            SqlLoader.Load("Categories.GetByUserId"), new { UserId = userId }, _session.Transaction);

    public Task<bool> IsReferencedByTransactionsAsync(Guid categoryId) =>
        _session.Connection.ExecuteScalarAsync<bool>(
            SqlLoader.Load("Categories.IsReferencedByTransactions"), new { CategoryId = categoryId }, _session.Transaction);

    public Task AddAsync(Category category) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("Categories.Insert"), category, _session.Transaction);

    public Task UpdateAsync(Category category) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("Categories.Update"), category, _session.Transaction);

    public Task DeleteAsync(Guid id) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("Categories.Delete"), new { Id = id }, _session.Transaction);
}
