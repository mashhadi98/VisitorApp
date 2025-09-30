using VisitorApp.Application.Features.Catalog.Categories.Update;
using VisitorApp.Contract.Features.Catalog.Categories.Update;

namespace VisitorApp.API.Features.Catalog.Categories.Update;

public class UpdateCategoryHandler : PutEndpoint<UpdateCategoryRequest, UpdateCategoryCommandRequest, UpdateCategoryCommandResponse>
{
    public override string? RolesAccess => "";
    public UpdateCategoryHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
} 