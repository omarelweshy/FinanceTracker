using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Interfaces;

public class TransactionFilter
{
    public Guid? AccountId { get; init; }
    public Guid? CategoryId { get; init; }
    public TransactionType? Type { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}