namespace FinanceTracker.Domain.Events;

public record TransactionCreatedEvent(
    Guid TransactionId,
    Guid UserId,
    Guid AccountId,
    Guid? CategoryId,
    string Type,
    decimal Amount,
    DateTime TransactionDate) : IEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
