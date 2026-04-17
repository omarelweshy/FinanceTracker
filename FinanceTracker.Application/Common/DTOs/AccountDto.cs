namespace FinanceTracker.Application.Common.DTOs;

public record AccountDto(Guid Id, string Name, string Type, decimal Balance, string Currency, bool IsActive, DateTime CreatedAt);
