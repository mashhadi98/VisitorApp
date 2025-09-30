using VisitorApp.Application.Features.Orders.GetMyOrders;
using VisitorApp.Contract.Features.Orders.GetMyOrders;

namespace VisitorApp.API.Features.Orders.GetMyOrders;

public class GetMyOrdersHandler : GetEndpoint<GetMyOrdersRequest, GetMyOrdersQueryRequest, PaginatedResponse<GetMyOrdersQueryResponse>>
{
    public override string? RolesAccess => ""; // هر کاربر احراز هویت شده می‌تواند سفارشات خود را ببیند
    
    public GetMyOrdersHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}
