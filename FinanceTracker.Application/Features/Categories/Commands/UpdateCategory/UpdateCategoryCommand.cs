using FinanceTracker.Application.Common.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(Guid CategoryId, Guid UserId, string Name, string? Icon) : IRequest<CategoryDto>;
