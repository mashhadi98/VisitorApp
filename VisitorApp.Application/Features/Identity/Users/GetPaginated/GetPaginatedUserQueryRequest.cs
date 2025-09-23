using VisitorApp.Application.Common.Messaging;
using VisitorApp.Domain.Common.DTOs;

namespace VisitorApp.Application.Features.Identity.Users.GetPaginated;

public class GetPaginatedUserQueryRequest : Pagination<GetPaginatedUserFilter>, IRequestBase<PaginatedResponse<GetPaginatedUserQueryResponse>>
{
}

public class GetPaginatedUserFilter
{
    public string? SearchTerm { get; set; }
    public bool? IsActive { get; set; }
    public string? SortBy { get; set; } = "FirstName";
    public bool SortDescending { get; set; } = false;
} 