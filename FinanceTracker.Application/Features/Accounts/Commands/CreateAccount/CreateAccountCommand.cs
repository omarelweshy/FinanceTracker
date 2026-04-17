using FinanceTracker.Application.Common.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Accounts.Commands.CreateAccount;

public record CreateAccountCommand(Guid UserId, string Name, string Type, string Currency) : IRequest<AccountDto>;
