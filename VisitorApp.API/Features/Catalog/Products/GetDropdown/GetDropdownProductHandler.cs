using VisitorApp.Application.Features.Catalog.Products.GetDropdown;
using VisitorApp.Contract.Features.Catalog.Products.GetDropdown;

namespace VisitorApp.API.Features.Catalog.Products.GetDropdown;

public class GetDropdownProductHandler : DropdownEndpointWithoutRequest<GetDropdownProductRequest, GetDropdownProductQueryRequest>
{
    public override string? RolesAccess => "";

    public GetDropdownProductHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender)
    {
    }
}