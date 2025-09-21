using VisitorApp.Application.Features.Catalog.Categories.Delete;

namespace VisitorApp.API.Features.Catalog.Categories.Delete;

public class DeleteCategoryHandler : DeleteEndpoint<DeleteCategoryRequest, DeleteCategoryCommandRequest, DeleteCategoryCommandResponse>
{
    public override string? RolesAccess => "";
    public DeleteCategoryHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
} 