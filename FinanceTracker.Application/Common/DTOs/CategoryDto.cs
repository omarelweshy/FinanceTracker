namespace FinanceTracker.Application.Common.DTOs;

public record CategoryDto(Guid Id, string Name, string Type, string? Icon, bool IsDefault);
