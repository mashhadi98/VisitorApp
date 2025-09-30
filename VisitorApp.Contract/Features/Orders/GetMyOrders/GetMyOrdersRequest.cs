using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Orders.GetMyOrders;

public class GetMyOrdersRequest() : PaginatedRequestBase("Orders/my-orders", ApiTypes.Get)
{
}
