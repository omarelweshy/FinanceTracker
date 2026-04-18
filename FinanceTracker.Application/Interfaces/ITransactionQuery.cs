using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Interfaces;

public interface ITransactionQuery
{
    Task<TransactionDto?> GetByIdAsync(Guid id, Guid userId);
    Task<PagedResult<TransactionDto>> GetFilteredAsync(Guid userId, TransactionFilter filter);
}
