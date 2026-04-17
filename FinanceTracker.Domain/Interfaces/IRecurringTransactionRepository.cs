using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Interfaces;

public interface IRecurringTransactionRepository
{
    Task<RecurringTransaction?> GetByIdAsync(Guid id);
    Task<IEnumerable<RecurringTransaction>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<RecurringTransaction>> GetPendingAsync(DateTime asOf);
    Task AddAsync(RecurringTransaction recurringTransaction);
    Task UpdateAsync(RecurringTransaction recurringTransaction);
    Task DeleteAsync(Guid id);
}