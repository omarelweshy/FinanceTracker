namespace FinanceTracker.Domain.Events;

public record UserRegisteredEvent(Guid UserId, string Email, string FullName, string Currency) : IEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
