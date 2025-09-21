using VisitorApp.Application.Common.Messaging;
using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog.Categories.Delete;

public class DeleteCategoryCommandHandler(IRepository<Category> repository, IEnumerable<IValidatorService<DeleteCategoryCommandRequest, DeleteCategoryCommandResponse>> validators) : RequestHandlerBase<DeleteCategoryCommandRequest, DeleteCategoryCommandResponse>(validators)
{
    public override async Task<DeleteCategoryCommandResponse> Handler(DeleteCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var category = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            throw new ArgumentException("Category not found", nameof(request.Id));
        }

        await repository.DeleteAsync(entity: category, autoSave: true, cancellationToken: cancellationToken);

        return new DeleteCategoryCommandResponse { Id = request.Id };
    }
} 