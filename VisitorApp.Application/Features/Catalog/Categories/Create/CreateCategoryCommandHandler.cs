using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Catalog.Categories.Create;
using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog.Categories.Create;

public class CreateCategoryCommandHandler(IRepository<Category> repository, IMapper _mapper, IEnumerable<IValidatorService<CreateCategoryCommandRequest, CreateCategoryCommandResponse>> validators) : RequestHandlerBase<CreateCategoryCommandRequest, CreateCategoryCommandResponse>(validators)
{
    public override async Task<CreateCategoryCommandResponse> Handler(CreateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var item = _mapper.Map<Category>(request);
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        await repository.AddAsync(entity: item, autoSave: true, cancellationToken: cancellationToken);

        var result = _mapper.Map<CreateCategoryCommandResponse>(item);
        return result;
    }
} 