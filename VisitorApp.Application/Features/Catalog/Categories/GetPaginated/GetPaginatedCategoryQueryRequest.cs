namespace VisitorApp.Application.Features.Catalog.Categories.GetPaginated;

public class GetPaginatedCategoryQueryRequest : Pagination<GetPaginatedCategoryQueryFilter>, IRequestBase<PaginatedResponse<GetPaginatedCategoryQueryResponse>>
{
}

public class GetPaginatedCategoryQueryFilter
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
} 