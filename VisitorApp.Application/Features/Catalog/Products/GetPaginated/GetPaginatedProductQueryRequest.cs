namespace VisitorApp.Application.Features.Catalog.Products.GetPaginated;

public class GetPaginatedProductQueryRequest : Pagination<GetAllGroupsQueryFilter>, IRequestBase<PaginatedResponse<GetPaginatedProductQueryResponse>>
{
}

public class GetAllGroupsQueryFilter
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}