using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var existing = await _categoryRepository.GetByUserIdAsync(request.UserId);
        if (existing.Any(c => c.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
            throw new DomainException($"A category named '{request.Name}' already exists.");

        var type = Enum.Parse<CategoryType>(request.Type, ignoreCase: true);
        var category = Category.Create(request.UserId, request.Name, type, request.Icon);

        await _categoryRepository.AddAsync(category);

        return new CategoryDto(category.Id, category.Name, category.Type.ToString(), category.Icon, category.IsDefault);
    }
}
