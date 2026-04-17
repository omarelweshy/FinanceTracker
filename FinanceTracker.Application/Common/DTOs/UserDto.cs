namespace FinanceTracker.Application.Common.DTOs;

public record UserDto(Guid Id, string Email, string FullName, string Currency);
