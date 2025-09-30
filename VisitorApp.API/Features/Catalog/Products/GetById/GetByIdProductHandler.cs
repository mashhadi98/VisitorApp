using VisitorApp.Application.Features.Catalog.Products.GetById;
using VisitorApp.Contract.Features.Catalog.Products.GetById;

namespace VisitorApp.API.Features.Catalog.Products.GetById;

public class GetByIdProductHandler : GetEndpoint<GetByIdProductRequest, GetByIdProductQueryRequest, GetByIdProductQueryResponse>
{
    public override string? RolesAccess => "";
    public GetByIdProductHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}