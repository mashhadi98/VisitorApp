using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog.Categories.ChangeState;

public class ChangeStateCategoryCommandHandler(IRepository<Category> repository) : RequestHandlerBase<ChangeStateCategoryCommandRequest, ChangeStateCategoryCommandResponse>()
{
    public override async Task<ChangeStateCategoryCommandResponse> Handler(ChangeStateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var category = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            throw new ArgumentException("Category not found", nameof(request.Id));
        }

        category.IsActive = request.IsActive;
        await repository.UpdateAsync(entity: category, autoSave: true, cancellationToken: cancellationToken);

        return new ChangeStateCategoryCommandResponse 
        { 
            Id = category.Id, 
            IsActive = category.IsActive 
        };
    }
} 