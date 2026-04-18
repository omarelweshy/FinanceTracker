using FinanceTracker.Application.Common.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Transfers.Commands.CreateTransfer;

public record CreateTransferCommand(
    Guid UserId,
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount,
    DateTime TransferDate,
    string? Note
) : IRequest<TransferDto>;
