using VisitorApp.Application.Features.Catalog.Categories.ChangeState;
using VisitorApp.Contract.Features.Catalog.Categories.ChangeState;

namespace VisitorApp.API.Features.Catalog.Categories.ChangeState;

public class ChangeStateCategoryHandler : PatchEndpoint<ChangeStateCategoryRequest, ChangeStateCategoryCommandRequest, ChangeStateCategoryCommandResponse>
{
    public override string? RolesAccess => "";
    public ChangeStateCategoryHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
} 