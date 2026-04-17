using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Category
{
    private Category()
    {
    }

    public Guid Id { get; private set; }
    public Guid? UserId { get; private set; }
    public string Name { get; private set; }
    public CategoryType Type { get; private set; }
    public string? Icon { get; private set; }
    public bool IsDefault { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static Category Create(Guid userId, string name, CategoryType type, string? icon = null)
    {
        return new Category
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = name,
            Type = type,
            Icon = icon,
            IsDefault = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string name, string? icon)
    {
        Name = name;
        Icon = icon;
    }

    public static Category CreateDefault(string name, CategoryType type, string? icon = null)
    {
        return new Category
        {
            Id = Guid.NewGuid(),
            UserId = null,
            Name = name,
            Type = type,
            Icon = icon,
            IsDefault = true,
            CreatedAt = DateTime.UtcNow
        };
    }
}