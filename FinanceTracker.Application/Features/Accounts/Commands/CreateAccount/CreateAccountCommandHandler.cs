using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, AccountDto>
{
    private readonly IAccountRepository _accountRepository;

    public CreateAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        if (await _accountRepository.CountActiveAsync(request.UserId) >= 20)
            throw new DomainException("Maximum of 20 active accounts allowed.");

        if (await _accountRepository.ExistsAsync(request.UserId, request.Name))
            throw new DomainException($"An account named '{request.Name}' already exists.");

        var type = Enum.Parse<AccountType>(request.Type, ignoreCase: true);
        var account = Account.Create(request.UserId, request.Name, type, request.Currency);

        await _accountRepository.AddAsync(account);

        return new AccountDto(account.Id, account.Name, account.Type.ToString(), account.Balance, account.Currency, account.IsActive, account.CreatedAt);
    }
}
