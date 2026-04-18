using FinanceTracker.Application.Common.DTOs;

namespace FinanceTracker.Application.Interfaces;

public interface ITransferQuery
{
    Task<TransferDto?> GetByIdAsync(Guid id, Guid userId);
    Task<PagedResult<TransferDto>> GetByUserIdAsync(Guid userId, int page, int pageSize);
}
