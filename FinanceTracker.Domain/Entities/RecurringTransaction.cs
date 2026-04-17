using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class RecurringTransaction
{
    private RecurringTransaction()
    {
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }
    public decimal Amount { get; private set; }
    public string? Description { get; private set; }
    public Frequency Frequency { get; private set; }
    public DateTime NextRunDate { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static RecurringTransaction Create(Guid userId, Guid accountId, Guid categoryId,
        decimal amount, Frequency frequency, DateTime nextRunDate, string? description = null)
    {
        return new RecurringTransaction
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AccountId = accountId,
            CategoryId = categoryId,
            Amount = amount,
            Description = description,
            Frequency = frequency,
            NextRunDate = nextRunDate,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void AdvanceNextRunDate()
    {
        NextRunDate = Frequency switch
        {
            Frequency.Daily => NextRunDate.AddDays(1),
            Frequency.Weekly => NextRunDate.AddDays(7),
            Frequency.Biweekly => NextRunDate.AddDays(14),
            Frequency.Monthly => NextRunDate.AddMonths(1),
            Frequency.Yearly => NextRunDate.AddYears(1),
            _ => throw new InvalidOperationException($"Unknown frequency: {Frequency}")
        };
    }

    public void Pause()
    {
        IsActive = false;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}