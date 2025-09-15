using VisitorApp.Application.Features.Catalog.Products.Update;

namespace VisitorApp.API.Features.Catalog.Products.Update;

public class UpdateProductHandler : PutEndpoint<UpdateProductRequest, UpdateProductCommandRequest, UpdateProductCommandResponse>
{
    public override string? RolesAccess => "";
    public UpdateProductHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}