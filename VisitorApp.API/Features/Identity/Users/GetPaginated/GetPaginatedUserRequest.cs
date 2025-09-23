namespace VisitorApp.API.Features.Identity.Users.GetPaginated;

public class GetPaginatedUserRequest : PaginatedRequestBase<GetPaginatedUserFilter>
{
    public GetPaginatedUserRequest() : base("Users") { }
}

public class GetPaginatedUserFilter
{
    public string? SearchTerm { get; set; }
    public bool? IsActive { get; set; }
    public string? SortBy { get; set; } = "FirstName";
    public bool SortDescending { get; set; } = false;
} 