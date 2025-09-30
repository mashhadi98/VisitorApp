using VisitorApp.Contract.Features.Customers.GetPaginated;

namespace VisitorApp.Application.Features.Customers.GetPaginated;

public class GetPaginatedCustomerQueryRequest : Pagination<GetPaginatedCustomerFilterRequest>, IRequestBase<PaginatedResponse<GetPaginatedCustomerQueryResponse>>
{
}

public class GetPaginatedCustomerFilterRequest
{
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CompanyName { get; set; }
    public bool? IsTemporary { get; set; }
}
