using VisitorApp.Contract.Features.Customers.GetById;

namespace VisitorApp.Application.Features.Customers.GetById;

public class GetByIdCustomerQueryRequest : IRequestBase<GetByIdCustomerQueryResponse>
{
    public Guid Id { get; set; }
}
