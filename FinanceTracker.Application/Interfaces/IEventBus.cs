using FinanceTracker.Domain.Events;

namespace FinanceTracker.Application.Interfaces;

public interface IEventBus
{
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IEvent;
}
