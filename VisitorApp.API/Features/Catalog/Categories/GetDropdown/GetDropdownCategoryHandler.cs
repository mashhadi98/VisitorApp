using VisitorApp.Application.Features.Catalog.Categories.GetDropdown;

namespace VisitorApp.API.Features.Catalog.Categories.GetDropdown;

public class GetDropdownCategoryHandler : DropdownEndpoint<GetDropdownCategoryRequest, GetDropdownCategoryQueryRequest>
{
    public override string? RolesAccess => "";
    public GetDropdownCategoryHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
} 