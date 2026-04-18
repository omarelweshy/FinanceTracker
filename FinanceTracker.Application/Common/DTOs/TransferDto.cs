namespace FinanceTracker.Application.Common.DTOs;

public record TransferDto
{
    public Guid Id { get; init; }
    public Guid FromTransactionId { get; init; }
    public Guid ToTransactionId { get; init; }
    public Guid FromAccountId { get; init; }
    public string FromAccountName { get; init; } = string.Empty;
    public Guid ToAccountId { get; init; }
    public string ToAccountName { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string? Note { get; init; }
    public DateTime CreatedAt { get; init; }
}
