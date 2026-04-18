namespace FinanceTracker.Application.Common.DTOs;

public record PagedResult<T>(IEnumerable<T> Items, int Total, int Page, int PageSize);
