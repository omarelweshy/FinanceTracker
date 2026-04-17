using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Accounts.Queries.GetAccount;

public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, AccountDto>
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountDto> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId)
            ?? throw new DomainException("Account not found.");

        if (account.UserId != request.UserId)
            throw new DomainException("Account not found.");

        return new AccountDto(account.Id, account.Name, account.Type.ToString(), account.Balance, account.Currency, account.IsActive, account.CreatedAt);
    }
}
