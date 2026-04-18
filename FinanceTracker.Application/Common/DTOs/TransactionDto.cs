using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Common.DTOs;

public record TransactionDto
{
    public Guid Id { get; init; }
    public Guid AccountId { get; init; }
    public string AccountName { get; init; } = string.Empty;
    public Guid? CategoryId { get; init; }
    public string? CategoryName { get; init; }
    public TransactionType Type { get; init; }
    public decimal Amount { get; init; }
    public string? Description { get; init; }
    public DateTime TransactionDate { get; init; }
    public DateTime CreatedAt { get; init; }
}
