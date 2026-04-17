using FinanceTracker.Application.Common.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(Guid UserId, string Name, string Type, string? Icon) : IRequest<CategoryDto>;
