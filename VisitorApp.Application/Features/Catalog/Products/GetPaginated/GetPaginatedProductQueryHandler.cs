using VisitorApp.Contract.Features.Catalog.Products.GetPaginated;
using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog.Products.GetPaginated;

public class GetPaginatedProductQueryHandler(IRepository<Product> repository) : RequestHandlerBase<GetPaginatedProductQueryRequest, PaginatedResponse<GetPaginatedProductQueryResponse>>()
{
    public override Task<PaginatedResponse<GetPaginatedProductQueryResponse>> Handler(GetPaginatedProductQueryRequest request, CancellationToken cancellationToken)
    {
        Expression<Func<Product, bool>> predicate = x => true;
        if (request.Filter != null)
        {
            if (!string.IsNullOrWhiteSpace(request.Filter.Description))
            {
                predicate = predicate.And(p => p.Description != null && p.Description.Contains(request.Filter.Description));
            }

            if (!string.IsNullOrWhiteSpace(request.Filter.Title))
            {
                predicate = predicate.And(p => p.Title != null && p.Title.Contains(request.Filter.Title));
            }

            if (request.Filter.IsActive != null)
            {
                predicate = predicate.And(p => p.IsActive.Equals(request.Filter.IsActive));
            }
        }
        var result = repository.GetListPaginatedAsync(request, predicate, x => new GetPaginatedProductQueryResponse
        {
            Description = x.Description,
            Id = x.Id,
            IsActive = x.IsActive,
            Title = x.Title,
            CategoryId = x.CategoryId,
            CategoryName = x.Category != null ? x.Category.Name : null,
            Price = x.Price,
            ImageUrl = x.ImageUrl,
            ImageFileName = x.ImageFileName,
            ImageFileSize = x.ImageFileSize,
            HasImage = !string.IsNullOrEmpty(x.ImagePath)
        }, cancellationToken);

        return result;
    }
}