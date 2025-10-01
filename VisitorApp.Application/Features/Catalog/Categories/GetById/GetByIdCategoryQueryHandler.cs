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

        return new GetByIdCategoryQueryResponse
        {
            CreatedAt = item.CreatedAt,
            Description = item.Description,
            Id = item.Id,
            IsActive = item.IsActive,
            Name = item.Name,
            UpdatedAt = item.UpdatedAt
        };
    }
}