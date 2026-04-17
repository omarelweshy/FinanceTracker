namespace FinanceTracker.Application.Common.DTOs;

public record AuthResponse(Guid UserId, string Email, string FullName, string Currency, string Token);
