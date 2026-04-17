namespace FinanceTracker.Application.Interfaces;

public interface ICategorySeeder
{
    Task SeedAsync(Guid userId);
}
