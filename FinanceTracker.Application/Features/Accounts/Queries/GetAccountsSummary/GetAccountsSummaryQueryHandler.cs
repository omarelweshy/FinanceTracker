using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Accounts.Queries.GetAccountsSummary;

public class GetAccountsSummaryQueryHandler : IRequestHandler<GetAccountsSummaryQuery, AccountSummaryDto>
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountsSummaryQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountSummaryDto> Handle(GetAccountsSummaryQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _accountRepository.GetByUserIdAsync(request.UserId);
        var list = accounts.ToList();

        return new AccountSummaryDto(
            TotalBalance: list.Sum(a => a.Balance),
            Currency: list.FirstOrDefault()?.Currency ?? "USD",
            ActiveAccountsCount: list.Count);
    }
}
