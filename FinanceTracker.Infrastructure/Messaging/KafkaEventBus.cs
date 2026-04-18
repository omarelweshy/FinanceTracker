using System.Text.Json;
using Confluent.Kafka;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Events;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Infrastructure.Messaging;

public class KafkaEventBus : IEventBus, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaEventBus> _logger;

    public KafkaEventBus(KafkaSettings settings, ILogger<KafkaEventBus> logger)
    {
        _logger = logger;
        var config = new ProducerConfig { BootstrapServers = settings.BootstrapServers };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IEvent
    {
        var topic = typeof(T).Name;
        var payload = JsonSerializer.Serialize(@event);

        var result = await _producer.ProduceAsync(topic, new Message<string, string>
        {
            Key = @event.EventId.ToString(),
            Value = payload
        }, cancellationToken);

        _logger.LogInformation(
            "Published {EventType} to topic {Topic} partition {Partition} offset {Offset}",
            typeof(T).Name, result.Topic, result.Partition.Value, result.Offset.Value);
    }

    public void Dispose() => _producer.Dispose();
}
