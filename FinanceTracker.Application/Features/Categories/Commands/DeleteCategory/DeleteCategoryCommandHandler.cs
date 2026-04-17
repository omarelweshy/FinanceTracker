using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId)
            ?? throw new DomainException("Category not found.");

        if (category.IsDefault)
            throw new DomainException("Default categories cannot be deleted.");

        if (category.UserId != request.UserId)
            throw new DomainException("Category not found.");

        if (await _categoryRepository.IsReferencedByTransactionsAsync(request.CategoryId))
            throw new DomainException("Category is in use by transactions and cannot be deleted.");

        await _categoryRepository.DeleteAsync(request.CategoryId);
    }
}
