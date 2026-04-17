namespace FinanceTracker.Domain.ValueObjects;

public record DateRange
{
    public DateRange(DateTime start, DateTime end)
    {
        if (end < start) throw new ArgumentException("End date must be after start date.");
        Start = start;
        End = end;
    }

    public DateTime Start { get; }
    public DateTime End { get; }

    public bool Contains(DateTime date)
    {
        return date >= Start && date <= End;
    }

    public static DateRange ForMonth(int month, int year)
    {
        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1).AddTicks(-1);
        return new DateRange(start, end);
    }
}