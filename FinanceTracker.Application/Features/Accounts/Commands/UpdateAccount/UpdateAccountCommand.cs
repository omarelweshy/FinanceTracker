using FinanceTracker.Application.Common.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Accounts.Commands.UpdateAccount;

public record UpdateAccountCommand(Guid AccountId, Guid UserId, string Name, string Type) : IRequest<AccountDto>;
