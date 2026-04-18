using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Events;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Features.Transactions.Events;

public class TransactionCreatedEventHandler : IEventHandler<TransactionCreatedEvent>
{
    private readonly ILogger<TransactionCreatedEventHandler> _logger;

    public TransactionCreatedEventHandler(ILogger<TransactionCreatedEventHandler> logger) => _logger = logger;

    public Task HandleAsync(TransactionCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Transaction {TransactionId} created — type={Type} amount={Amount} account={AccountId}. Ready for budget check in Phase 3.",
            @event.TransactionId, @event.Type, @event.Amount, @event.AccountId);
        return Task.CompletedTask;
    }
}
