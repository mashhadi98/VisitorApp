using VisitorApp.Contract.Features.Catalog.Categories.GetById;
using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog.Categories.GetById;

public class GetByIdCategoryQueryHandler(IRepository<Category> repository, IMapper _mapper) : RequestHandlerBase<GetByIdCategoryQueryRequest, GetByIdCategoryQueryResponse>()
{
    public override async Task<GetByIdCategoryQueryResponse> Handler(GetByIdCategoryQueryRequest request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (item == null)
        {
            throw new ArgumentException("Category not found", nameof(request.Id));
        }

        var result = _mapper.Map<GetByIdCategoryQueryResponse>(item);

        return result;
    }
} 