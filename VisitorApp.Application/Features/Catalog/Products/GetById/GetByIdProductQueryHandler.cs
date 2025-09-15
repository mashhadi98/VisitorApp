using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog.Products.GetById;

public class GetByIdProductQueryHandler(IRepository<Product> repository, IMapper _mapper) : RequestHandlerBase<GetByIdProductQueryRequest, GetByIdProductQueryResponse>()
{
    public override async Task<GetByIdProductQueryResponse> Handler(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (item == null)
        {
            throw new ArgumentException("Product not found", nameof(request.Id));
        }

        var result = _mapper.Map<GetByIdProductQueryResponse>(item);

        return result;
    }
}