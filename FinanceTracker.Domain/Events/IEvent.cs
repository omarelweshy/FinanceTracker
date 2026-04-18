namespace FinanceTracker.Domain.Events;

public interface IEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
    string EventType => GetType().Name;
}
