using System.Security.Claims;
using FinanceTracker.Application.Features.Accounts.Commands.CreateAccount;
using FinanceTracker.Application.Features.Accounts.Commands.DeleteAccount;
using FinanceTracker.Application.Features.Accounts.Commands.UpdateAccount;
using FinanceTracker.Application.Features.Accounts.Queries.GetAccount;
using FinanceTracker.Application.Features.Accounts.Queries.GetAccounts;
using FinanceTracker.Application.Features.Accounts.Queries.GetAccountsSummary;
using MediatR;

namespace FinanceTracker.Api.Endpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/accounts").RequireAuthorization();

        group.MapPost("/", async (CreateAccountRequest request, ClaimsPrincipal principal, IMediator mediator) =>
        {
            var userId = GetUserId(principal);
            var result = await mediator.Send(new CreateAccountCommand(userId, request.Name, request.Type, request.Currency));
            return Results.Created($"/api/accounts/{result.Id}", result);
        });

        group.MapGet("/", async (ClaimsPrincipal principal, IMediator mediator) =>
        {
            var userId = GetUserId(principal);
            var result = await mediator.Send(new GetAccountsQuery(userId));
            return Results.Ok(result);
        });

        group.MapGet("/summary", async (ClaimsPrincipal principal, IMediator mediator) =>
        {
            var userId = GetUserId(principal);
            var result = await mediator.Send(new GetAccountsSummaryQuery(userId));
            return Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, ClaimsPrincipal principal, IMediator mediator) =>
        {
            var userId = GetUserId(principal);
            var result = await mediator.Send(new GetAccountQuery(id, userId));
            return Results.Ok(result);
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateAccountRequest request, ClaimsPrincipal principal, IMediator mediator) =>
        {
            var userId = GetUserId(principal);
            var result = await mediator.Send(new UpdateAccountCommand(id, userId, request.Name, request.Type));
            return Results.Ok(result);
        });

        group.MapDelete("/{id:guid}", async (Guid id, ClaimsPrincipal principal, IMediator mediator) =>
        {
            var userId = GetUserId(principal);
            await mediator.Send(new DeleteAccountCommand(id, userId));
            return Results.NoContent();
        });
    }

    private static Guid GetUserId(ClaimsPrincipal principal) =>
        Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
}

public record CreateAccountRequest(string Name, string Type, string Currency);
public record UpdateAccountRequest(string Name, string Type);
