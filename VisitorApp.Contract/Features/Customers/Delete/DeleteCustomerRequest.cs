using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Customers.Delete;

public class DeleteCustomerRequest() : RequestBase("Customers/{Id}", ApiTypes.Delete)
{
    public Guid Id { get; set; }
}
