using VisitorApp.Application.Features.Catalog.Categories.Create;

namespace VisitorApp.API.Features.Catalog.Categories.Create;

public class CreateCategoryHandler : PostEndpoint<CreateCategoryRequest, CreateCategoryCommandRequest, CreateCategoryCommandResponse>
{
    public override string? RolesAccess => "";
    public CreateCategoryHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
} 