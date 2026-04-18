using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Transfers.Queries.GetTransfers;

public class GetTransfersHandler : IRequestHandler<GetTransfersQuery, PagedResult<TransferDto>>
{
    private readonly ITransferQuery _query;

    public GetTransfersHandler(ITransferQuery query) => _query = query;

    public Task<PagedResult<TransferDto>> Handle(GetTransfersQuery request, CancellationToken cancellationToken) =>
        _query.GetByUserIdAsync(request.UserId, request.Page, request.PageSize);
}
