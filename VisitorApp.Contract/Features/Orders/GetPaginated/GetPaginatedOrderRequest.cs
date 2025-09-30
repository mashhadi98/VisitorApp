using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Orders.GetPaginated;

public class GetPaginatedOrderRequest() : PaginatedRequestBase("Orders", ApiTypes.Get)
{
}
