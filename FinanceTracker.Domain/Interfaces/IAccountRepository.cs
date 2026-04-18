using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id);
    Task<Account?> GetByIdForUpdateAsync(Guid id);
    Task<IEnumerable<Account>> GetByUserIdAsync(Guid userId);
    Task<bool> ExistsAsync(Guid userId, string name);
    Task<int> CountActiveAsync(Guid userId);
    Task AddAsync(Account account);
    Task UpdateAsync(Account account);
    Task UpdateBalanceAsync(Guid id, decimal balance);
    Task DeleteAsync(Guid id);
}
