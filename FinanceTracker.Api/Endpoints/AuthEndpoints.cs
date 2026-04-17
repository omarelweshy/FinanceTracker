using System.Security.Claims;
using FinanceTracker.Application.Features.Auth.Commands.Login;
using FinanceTracker.Application.Features.Auth.Commands.Register;
using FinanceTracker.Application.Features.Auth.Commands.UpdateProfile;
using FinanceTracker.Application.Features.Auth.Queries.GetCurrentUser;
using MediatR;

namespace FinanceTracker.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/register", async (RegisterCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        });

        group.MapPost("/login", async (LoginCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        });

        group.MapGet("/me", async (ClaimsPrincipal principal, IMediator mediator) =>
        {
            var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await mediator.Send(new GetCurrentUserQuery(userId));
            return Results.Ok(result);
        }).RequireAuthorization();

        group.MapPut("/me", async (UpdateProfileRequest request, ClaimsPrincipal principal, IMediator mediator) =>
        {
            var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await mediator.Send(new UpdateProfileCommand(userId, request.FullName, request.Currency));
            return Results.Ok(result);
        }).RequireAuthorization();
    }
}

public record UpdateProfileRequest(string FullName, string Currency);
