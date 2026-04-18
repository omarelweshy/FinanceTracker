using System.Security.Claims;
using FinanceTracker.Application.Features.Transactions.Commands.CreateTransaction;
using FinanceTracker.Application.Features.Transactions.Commands.DeleteTransaction;
using FinanceTracker.Application.Features.Transactions.Commands.UpdateTransaction;
using FinanceTracker.Application.Features.Transactions.Queries.GetTransaction;
using FinanceTracker.Application.Features.Transactions.Queries.GetTransactions;
using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Api.Endpoints;

public static class TransactionEndpoints
{
    public static void MapTransactionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/transactions").RequireAuthorization();

        group.MapPost("/", async (CreateTransactionRequest req, ClaimsPrincipal principal, IMediator mediator) =>
        {
            var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await mediator.Send(new CreateTransactionCommand(
                userId, req.AccountId, req.CategoryId, req.Type, req.Amount, req.TransactionDate, req.Description));
            return Results.Created($"/api/transactions/{result.Id}", result);
        });

        group.MapGet("/", async (
            ClaimsPrincipal principal, IMediator mediator,
            Guid? accountId, Guid? categoryId, TransactionType? type,
            DateTime? from, DateTime? to, int page = 1, int pageSize = 20) =>
        {
            var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await mediator.Send(new GetTransactionsQuery(userId, accountId, categoryId, type, from, to, page, pageSize));
            return Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, ClaimsPrincipal principal, IMediator mediator) =>
        {
            var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await mediator.Send(new GetTransactionQuery(id, userId));
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateTransactionRequest req, ClaimsPrincipal principal, IMediator mediator) =>
        {
            var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await mediator.Send(new UpdateTransactionCommand(
                id, userId, req.Amount, req.CategoryId, req.TransactionDate, req.Description));
            return Results.Ok(result);
        });

        group.MapDelete("/{id:guid}", async (Guid id, ClaimsPrincipal principal, IMediator mediator) =>
        {
            var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await mediator.Send(new DeleteTransactionCommand(id, userId));
            return Results.NoContent();
        });
    }
}

public record CreateTransactionRequest(
    Guid AccountId, Guid CategoryId, TransactionType Type,
    decimal Amount, DateTime TransactionDate, string? Description);

public record UpdateTransactionRequest(
    decimal Amount, Guid? CategoryId, DateTime TransactionDate, string? Description);
