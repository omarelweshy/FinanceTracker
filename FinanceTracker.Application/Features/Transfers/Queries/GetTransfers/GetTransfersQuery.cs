using FinanceTracker.Application.Common.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Transfers.Queries.GetTransfers;

public record GetTransfersQuery(Guid UserId, int Page = 1, int PageSize = 20) : IRequest<PagedResult<TransferDto>>;
