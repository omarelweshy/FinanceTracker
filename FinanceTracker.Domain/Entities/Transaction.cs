using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Transaction
{
    private Transaction() { }

    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid? CategoryId { get; private set; }
    public TransactionType Type { get; private set; }
    public decimal Amount { get; private set; }
    public string? Description { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static Transaction Create(Guid accountId, Guid? categoryId, TransactionType type,
        decimal amount, DateTime transactionDate, string? description = null)
    {
        return new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = accountId,
            CategoryId = categoryId,
            Type = type,
            Amount = amount,
            Description = description,
            TransactionDate = transactionDate,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(decimal amount, Guid? categoryId, DateTime transactionDate, string? description)
    {
        Amount = amount;
        CategoryId = categoryId;
        TransactionDate = transactionDate;
        Description = description;
    }
}
