using VisitorApp.Application.Features.Catalog.Products.GetPaginated;

namespace VisitorApp.API.Features.Catalog.Products.GetPaginated;

public class GetPaginatedProductHandler : PaginatedEndpoint<GetPaginatedProductRequest, GetPaginatedProductQueryRequest, GetPaginatedProductQueryResponse>
{
    public override string? RolesAccess => "";
    public GetPaginatedProductHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}