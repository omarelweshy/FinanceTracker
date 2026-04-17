using MediatR;

namespace FinanceTracker.Application.Features.Accounts.Commands.DeleteAccount;

public record DeleteAccountCommand(Guid AccountId, Guid UserId) : IRequest;
