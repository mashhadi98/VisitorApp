using VisitorApp.Application.Features.Catalog.Products.ChangeState;

namespace VisitorApp.API.Features.Catalog.Products.ChangeState;

public class ChangeStateProductHandler : PutEndpoint<ChangeStateProductRequest, ChangeStateProductCommandRequest, ChangeStateProductCommandResponse>
{
    public override string? RolesAccess => "";
    
    public ChangeStateProductHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}