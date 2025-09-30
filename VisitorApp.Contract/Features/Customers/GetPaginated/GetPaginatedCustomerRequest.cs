using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Customers.GetPaginated;

public class GetPaginatedCustomerRequest() : PaginatedRequestBase("Customers", ApiTypes.Get)
{
}
