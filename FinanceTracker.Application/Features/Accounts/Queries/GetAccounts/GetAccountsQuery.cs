using FinanceTracker.Application.Common.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Accounts.Queries.GetAccounts;

public record GetAccountsQuery(Guid UserId) : IRequest<IEnumerable<AccountDto>>;
