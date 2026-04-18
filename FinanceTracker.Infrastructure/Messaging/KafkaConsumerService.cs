using System.Text.Json;
using Confluent.Kafka;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Infrastructure.Messaging;

public class KafkaConsumerService : BackgroundService
{
    private readonly KafkaSettings _settings;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<KafkaConsumerService> _logger;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    private static readonly string[] Topics = ["UserRegisteredEvent", "TransactionCreatedEvent"];

    public KafkaConsumerService(KafkaSettings settings, IServiceScopeFactory scopeFactory, ILogger<KafkaConsumerService> logger)
    {
        _settings = settings;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() => ConsumeLoop(stoppingToken), stoppingToken);
    }

    private async Task ConsumeLoop(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _settings.BootstrapServers,
            GroupId = _settings.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(Topics);
        _logger.LogInformation("Kafka consumer started, subscribed to: {Topics}", string.Join(", ", Topics));

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(stoppingToken);
                _logger.LogInformation("Consumed [{Topic}] key={Key}", result.Topic, result.Message.Key);

                using var scope = _scopeFactory.CreateScope();
                await DispatchAsync(result.Topic, result.Message.Value, scope.ServiceProvider, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ConsumeException ex) when (ex.Error.Code == ErrorCode.UnknownTopicOrPart)
            {
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kafka consumer error");
            }
        }

        consumer.Close();
        _logger.LogInformation("Kafka consumer stopped");
    }

    private async Task DispatchAsync(string topic, string json, IServiceProvider sp, CancellationToken ct)
    {
        switch (topic)
        {
            case "UserRegisteredEvent":
                var userEvent = JsonSerializer.Deserialize<UserRegisteredEvent>(json, _jsonOptions)!;
                await sp.GetRequiredService<IEventHandler<UserRegisteredEvent>>().HandleAsync(userEvent, ct);
                break;

            case "TransactionCreatedEvent":
                var txEvent = JsonSerializer.Deserialize<TransactionCreatedEvent>(json, _jsonOptions)!;
                await sp.GetRequiredService<IEventHandler<TransactionCreatedEvent>>().HandleAsync(txEvent, ct);
                break;

            default:
                _logger.LogWarning("No handler registered for topic {Topic}", topic);
                break;
        }
    }
}
