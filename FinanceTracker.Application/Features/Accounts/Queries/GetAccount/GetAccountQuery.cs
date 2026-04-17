using FinanceTracker.Application.Common.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Accounts.Queries.GetAccount;

public record GetAccountQuery(Guid AccountId, Guid UserId) : IRequest<AccountDto>;
