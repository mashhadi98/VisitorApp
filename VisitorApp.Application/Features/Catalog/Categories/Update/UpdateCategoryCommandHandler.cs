using VisitorApp.Application.Common.Messaging;
using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog.Categories.Update;

public class UpdateCategoryCommandHandler(IRepository<Category> repository, IMapper _mapper, IEnumerable<IValidatorService<UpdateCategoryCommandRequest, UpdateCategoryCommandResponse>> validators) : RequestHandlerBase<UpdateCategoryCommandRequest, UpdateCategoryCommandResponse>(validators)
{
    public override async Task<UpdateCategoryCommandResponse> Handler(UpdateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (item == null)
        {
            throw new ArgumentException("Category not found", nameof(request.Id));
        }
        
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            item.Name = request.Name;
        }
        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            item.Description = request.Description;
        }
        if (request.IsActive != null)
        {
            item.IsActive = request.IsActive ?? false;
        }

        await repository.UpdateAsync(entity: item, autoSave: true, cancellationToken: cancellationToken);
        var result = _mapper.Map<UpdateCategoryCommandResponse>(item);

        return result;
    }
} 