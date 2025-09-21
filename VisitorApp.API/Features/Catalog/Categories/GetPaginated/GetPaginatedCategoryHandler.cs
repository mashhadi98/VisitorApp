using VisitorApp.Application.Features.Catalog.Categories.GetPaginated;

namespace VisitorApp.API.Features.Catalog.Categories.GetPaginated;

public class GetPaginatedCategoryHandler : PaginatedEndpoint<GetPaginatedCategoryRequest, GetPaginatedCategoryQueryRequest, GetPaginatedCategoryQueryResponse>
{
    public override string? RolesAccess => "";
    public GetPaginatedCategoryHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
} 