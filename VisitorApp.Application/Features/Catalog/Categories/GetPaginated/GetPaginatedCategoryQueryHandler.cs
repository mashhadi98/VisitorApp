using VisitorApp.Contract.Features.Catalog.Categories.GetPaginated;
using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog.Categories.GetPaginated;

public class GetPaginatedCategoryQueryHandler(IRepository<Category> repository) : RequestHandlerBase<GetPaginatedCategoryQueryRequest, PaginatedResponse<GetPaginatedCategoryQueryResponse>>()
{
    public override Task<PaginatedResponse<GetPaginatedCategoryQueryResponse>> Handler(GetPaginatedCategoryQueryRequest request, CancellationToken cancellationToken)
    {
        Expression<Func<Category, bool>> predicate = x => true;
        if (request.Filter != null)
        {
            if (!string.IsNullOrWhiteSpace(request.Filter.Name))
            {
                predicate = predicate.And(p => p.Name != null && p.Name.Contains(request.Filter.Name));
            }

            if (!string.IsNullOrWhiteSpace(request.Filter.Description))
            {
                predicate = predicate.And(p => p.Description != null && p.Description.Contains(request.Filter.Description));
            }

            if (request.Filter.IsActive != null)
            {
                predicate = predicate.And(p => p.IsActive.Equals(request.Filter.IsActive));
            }
        }
        var result = repository.GetListPaginatedAsync(request, predicate, x => new GetPaginatedCategoryQueryResponse
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            IsActive = x.IsActive,
        }, cancellationToken);

        return result;
    }
} 