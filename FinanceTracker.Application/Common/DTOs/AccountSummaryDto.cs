namespace FinanceTracker.Application.Common.DTOs;

public record AccountSummaryDto(decimal TotalBalance, string Currency, int ActiveAccountsCount);
