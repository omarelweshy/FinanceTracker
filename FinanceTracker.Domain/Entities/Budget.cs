namespace FinanceTracker.Domain.Entities;

public class Budget
{
    private Budget()
    {
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid CategoryId { get; private set; }
    public decimal Amount { get; private set; }
    public int Month { get; private set; }
    public int Year { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static Budget Create(Guid userId, Guid categoryId, decimal amount, int month, int year)
    {
        return new Budget
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CategoryId = categoryId,
            Amount = amount,
            Month = month,
            Year = year,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateAmount(decimal amount)
    {
        Amount = amount;
    }
}