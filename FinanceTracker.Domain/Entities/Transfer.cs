namespace FinanceTracker.Domain.Entities;

public class Transfer
{
    private Transfer()
    {
    }

    public Guid Id { get; private set; }
    public Guid FromTransactionId { get; private set; }
    public Guid ToTransactionId { get; private set; }
    public decimal Amount { get; private set; }
    public string? Note { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static Transfer Create(Guid fromTransactionId, Guid toTransactionId, decimal amount, string? note = null)
    {
        return new Transfer
        {
            Id = Guid.NewGuid(),
            FromTransactionId = fromTransactionId,
            ToTransactionId = toTransactionId,
            Amount = amount,
            Note = note,
            CreatedAt = DateTime.UtcNow
        };
    }
}