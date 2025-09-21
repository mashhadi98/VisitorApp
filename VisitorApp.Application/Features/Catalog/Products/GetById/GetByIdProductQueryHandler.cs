using Microsoft.EntityFrameworkCore;
using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog.Products.GetById;

public class GetByIdProductQueryHandler(IRepository<Product> repository, IMapper _mapper) : RequestHandlerBase<GetByIdProductQueryRequest, GetByIdProductQueryResponse>()
{
    public override async Task<GetByIdProductQueryResponse> Handler(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
    {
        var item = await repository.GetQuery()
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (item == null)
        {
            throw new ArgumentException("Product not found", nameof(request.Id));
        }

        var result = new GetByIdProductQueryResponse
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            IsActive = item.IsActive,
            CategoryId = item.CategoryId,
            CategoryName = item.Category?.Name
        };

        return result;
    }
}