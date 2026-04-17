using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Interfaces;

public interface IBudgetRepository
{
    Task<Budget?> GetByIdAsync(Guid id);
    Task<IEnumerable<Budget>> GetByUserIdAsync(Guid userId, int month, int year);
    Task<bool> ExistsAsync(Guid userId, Guid categoryId, int month, int year);
    Task<decimal> GetSpentAmountAsync(Guid userId, Guid categoryId, int month, int year);
    Task AddAsync(Budget budget);
    Task UpdateAsync(Budget budget);
    Task DeleteAsync(Guid id);
}