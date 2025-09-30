using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Customers.GetById;

public class GetByIdCustomerRequest() : RequestBase("Customers/{Id}", ApiTypes.Get)
{
    public Guid Id { get; set; }
}
