using FinanceTracker.Application.Common.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Accounts.Queries.GetAccountsSummary;

public record GetAccountsSummaryQuery(Guid UserId) : IRequest<AccountSummaryDto>;
