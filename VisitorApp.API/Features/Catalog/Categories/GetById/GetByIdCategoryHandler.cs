using VisitorApp.Application.Features.Catalog.Categories.GetById;

namespace VisitorApp.API.Features.Catalog.Categories.GetById;

public class GetByIdCategoryHandler : GetEndpoint<GetByIdCategoryRequest, GetByIdCategoryQueryRequest, GetByIdCategoryQueryResponse>
{
    public override string? RolesAccess => "";
    public GetByIdCategoryHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
} 