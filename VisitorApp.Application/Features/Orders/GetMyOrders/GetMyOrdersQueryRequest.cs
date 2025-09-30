using VisitorApp.Contract.Features.Orders.GetMyOrders;

namespace VisitorApp.Application.Features.Orders.GetMyOrders;

public class GetMyOrdersQueryRequest : Pagination<GetMyOrdersFilterRequest>, IRequestBase<PaginatedResponse<GetMyOrdersQueryResponse>>
{
}

public class GetMyOrdersFilterRequest
{
    public string? OrderNumber { get; set; }
    public int? Status { get; set; }
    public DateTime? OrderDateFrom { get; set; }
    public DateTime? OrderDateTo { get; set; }
}
