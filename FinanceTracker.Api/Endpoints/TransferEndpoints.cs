using System.Security.Claims;
using FinanceTracker.Application.Features.Transfers.Commands.CreateTransfer;
using FinanceTracker.Application.Features.Transfers.Queries.GetTransfers;
using MediatR;

namespace FinanceTracker.Api.Endpoints;

public static class TransferEndpoints
{
    public static void MapTransferEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/transfers").RequireAuthorization();

        group.MapPost("/", async (CreateTransferRequest req, ClaimsPrincipal principal, IMediator mediator) =>
        {
            var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await mediator.Send(new CreateTransferCommand(
                userId, req.FromAccountId, req.ToAccountId, req.Amount, req.TransferDate, req.Note));
            return Results.Created($"/api/transfers/{result.Id}", result);
        });

        group.MapGet("/", async (ClaimsPrincipal principal, IMediator mediator, int page = 1, int pageSize = 20) =>
        {
            var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await mediator.Send(new GetTransfersQuery(userId, page, pageSize));
            return Results.Ok(result);
        });
    }
}

public record CreateTransferRequest(
    Guid FromAccountId, Guid ToAccountId,
    decimal Amount, DateTime TransferDate, string? Note);
