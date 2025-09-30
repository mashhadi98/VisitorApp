using VisitorApp.Contract.Features.Orders.GetPaginated;

namespace VisitorApp.Application.Features.Orders.GetPaginated;

public class GetPaginatedOrderQueryRequest : Pagination<GetPaginatedOrderFilterRequest>, IRequestBase<PaginatedResponse<GetPaginatedOrderQueryResponse>>
{
}

public class GetPaginatedOrderFilterRequest
{
    public string? OrderNumber { get; set; }
    public Guid? CustomerId { get; set; }
    public int? Status { get; set; }
    public DateTime? OrderDateFrom { get; set; }
    public DateTime? OrderDateTo { get; set; }
}
