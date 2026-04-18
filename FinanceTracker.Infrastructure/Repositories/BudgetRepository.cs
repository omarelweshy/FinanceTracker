using Dapper;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Database;

namespace FinanceTracker.Infrastructure.Repositories;

public class BudgetRepository : IBudgetRepository
{
    private readonly IDbSession _session;

    public BudgetRepository(IDbSession session) => _session = session;

    public Task<Budget?> GetByIdAsync(Guid id) =>
        _session.Connection.QueryFirstOrDefaultAsync<Budget>(
            SqlLoader.Load("Budgets.GetById"), new { Id = id }, _session.Transaction);

    public Task<IEnumerable<Budget>> GetByUserIdAsync(Guid userId, int month, int year) =>
        _session.Connection.QueryAsync<Budget>(
            SqlLoader.Load("Budgets.GetByUserId"), new { UserId = userId, Month = month, Year = year }, _session.Transaction);

    public Task<bool> ExistsAsync(Guid userId, Guid categoryId, int month, int year) =>
        _session.Connection.ExecuteScalarAsync<bool>(
            SqlLoader.Load("Budgets.Exists"), new { UserId = userId, CategoryId = categoryId, Month = month, Year = year }, _session.Transaction);

    public Task<decimal> GetSpentAmountAsync(Guid userId, Guid categoryId, int month, int year) =>
        _session.Connection.ExecuteScalarAsync<decimal>(
            SqlLoader.Load("Budgets.GetSpentAmount"), new { UserId = userId, CategoryId = categoryId, Month = month, Year = year }, _session.Transaction);

    public Task AddAsync(Budget budget) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("Budgets.Insert"), budget, _session.Transaction);

    public Task UpdateAsync(Budget budget) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("Budgets.Update"), budget, _session.Transaction);

    public Task DeleteAsync(Guid id) =>
        _session.Connection.ExecuteAsync(
            SqlLoader.Load("Budgets.Delete"), new { Id = id }, _session.Transaction);
}
