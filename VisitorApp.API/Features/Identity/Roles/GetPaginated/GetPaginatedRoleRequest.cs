namespace VisitorApp.API.Features.Identity.Roles.GetPaginated;

public class GetPaginatedRoleRequest : PaginatedRequestBase<GetPaginatedRoleFilter>
{
    public GetPaginatedRoleRequest() : base("Roles") { }
}

public class GetPaginatedRoleFilter
{
    public string? SearchTerm { get; set; }
    public bool? IsActive { get; set; }
    public string? SortBy { get; set; } = "Name";
    public bool SortDescending { get; set; } = false;
} 