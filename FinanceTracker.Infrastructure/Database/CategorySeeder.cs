using Dapper;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Infrastructure.Database;

public class CategorySeeder : ICategorySeeder
{
    private readonly IDbConnectionFactory _connectionFactory;

    private static readonly (string Name, CategoryType Type, string Icon)[] DefaultCategories =
    [
        ("Salary", CategoryType.Income, "💼"),
        ("Freelance", CategoryType.Income, "💻"),
        ("Investment Returns", CategoryType.Income, "📈"),
        ("Gifts Received", CategoryType.Income, "🎁"),
        ("Other Income", CategoryType.Income, "💰"),
        ("Groceries", CategoryType.Expense, "🛒"),
        ("Rent / Housing", CategoryType.Expense, "🏠"),
        ("Utilities", CategoryType.Expense, "💡"),
        ("Transportation", CategoryType.Expense, "🚗"),
        ("Dining Out", CategoryType.Expense, "🍽️"),
        ("Entertainment", CategoryType.Expense, "🎬"),
        ("Healthcare", CategoryType.Expense, "🏥"),
        ("Shopping", CategoryType.Expense, "🛍️"),
        ("Subscriptions", CategoryType.Expense, "📱"),
        ("Other Expenses", CategoryType.Expense, "📦")
    ];

    public CategorySeeder(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task SeedAsync(Guid userId)
    {
        using var connection = _connectionFactory.CreateConnection();
        foreach (var (name, type, icon) in DefaultCategories)
        {
            await connection.ExecuteAsync(
                """
                INSERT INTO categories (id, user_id, name, type, icon, is_default, created_at)
                VALUES (@Id, @UserId, @Name, @Type, @Icon, TRUE, @CreatedAt)
                """,
                new { Id = Guid.NewGuid(), UserId = userId, Name = name, Type = type.ToString(), Icon = icon, CreatedAt = DateTime.UtcNow });
        }
    }
}
