using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Interfaces;

public interface ITransferRepository
{
    Task<Transfer?> GetByIdAsync(Guid id);
    Task<IEnumerable<Transfer>> GetByUserIdAsync(Guid userId);
    Task AddAsync(Transfer transfer);
    Task DeleteAsync(Guid id);
}