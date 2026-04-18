using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Events;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Features.Auth.Events;

public class UserRegisteredEventHandler : IEventHandler<UserRegisteredEvent>
{
    private readonly ICategorySeeder _categorySeeder;
    private readonly ILogger<UserRegisteredEventHandler> _logger;

    public UserRegisteredEventHandler(ICategorySeeder categorySeeder, ILogger<UserRegisteredEventHandler> logger)
    {
        _categorySeeder = categorySeeder;
        _logger = logger;
    }

    public async Task HandleAsync(UserRegisteredEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Seeding default categories for new user {UserId}", @event.UserId);
        await _categorySeeder.SeedAsync(@event.UserId);
        _logger.LogInformation("Default categories seeded for user {UserId}", @event.UserId);
    }
}
