using FinanceTracker.Application.Common.DTOs;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId)
            ?? throw new DomainException("Category not found.");

        if (category.IsDefault)
            throw new DomainException("Default categories cannot be edited.");

        if (category.UserId != request.UserId)
            throw new DomainException("Category not found.");

        category.Update(request.Name, request.Icon);
        await _categoryRepository.UpdateAsync(category);

        return new CategoryDto(category.Id, category.Name, category.Type.ToString(), category.Icon, category.IsDefault);
    }
}
