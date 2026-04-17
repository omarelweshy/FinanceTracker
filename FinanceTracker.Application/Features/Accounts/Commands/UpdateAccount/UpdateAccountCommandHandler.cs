using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, AccountDto>
{
    private readonly IAccountRepository _accountRepository;

    public UpdateAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountDto> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId)
            ?? throw new DomainException("Account not found.");

        if (account.UserId != request.UserId)
            throw new DomainException("Account not found.");

        var type = Enum.Parse<AccountType>(request.Type, ignoreCase: true);
        account.Update(request.Name, type);
        await _accountRepository.UpdateAsync(account);

        return new AccountDto(account.Id, account.Name, account.Type.ToString(), account.Balance, account.Currency, account.IsActive, account.CreatedAt);
    }
}
