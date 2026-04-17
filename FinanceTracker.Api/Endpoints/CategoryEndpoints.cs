using System.Security.Claims;
using FinanceTracker.Application.Features.Categories.Commands.CreateCategory;
using FinanceTracker.Application.Features.Categories.Commands.DeleteCategory;
using FinanceTracker.Application.Features.Categories.Commands.UpdateCategory;
using FinanceTracker.Application.Features.Categories.Queries.GetCategories;
using MediatR;

namespace FinanceTracker.Api.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/categories").RequireAuthorization();

        group.MapPost("/", async (CreateCategoryRequest request, ClaimsPrincipal principal, IMediator mediator) =>
        {
            var userId = GetUserId(principal);
            var result = await mediator.Send(new CreateCategoryCommand(userId, request.Name, request.Type, request.Icon));
            return Results.Created($"/api/categories/{result.Id}", result);
        });

        group.MapGet("/", async (ClaimsPrincipal principal, IMediator mediator) =>
        {
            var userId = GetUserId(principal);
            var result = await mediator.Send(new GetCategoriesQuery(userId));
            return Results.Ok(result);
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateCategoryRequest request, ClaimsPrincipal principal, IMediator mediator) =>
        {
            var userId = GetUserId(principal);
            var result = await mediator.Send(new UpdateCategoryCommand(id, userId, request.Name, request.Icon));
            return Results.Ok(result);
        });

        group.MapDelete("/{id:guid}", async (Guid id, ClaimsPrincipal principal, IMediator mediator) =>
        {
            var userId = GetUserId(principal);
            await mediator.Send(new DeleteCategoryCommand(id, userId));
            return Results.NoContent();
        });
    }

    private static Guid GetUserId(ClaimsPrincipal principal) =>
        Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
}

public record CreateCategoryRequest(string Name, string Type, string? Icon);
public record UpdateCategoryRequest(string Name, string? Icon);
