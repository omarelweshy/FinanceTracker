namespace FinanceTracker.Infrastructure.Messaging;

public class KafkaSettings
{
    public string BootstrapServers { get; init; } = "localhost:9092";
    public string GroupId { get; init; } = "finance-tracker";
}
