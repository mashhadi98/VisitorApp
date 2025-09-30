using VisitorApp.Contract.Features.Customers.Delete;

namespace VisitorApp.Application.Features.Customers.Delete;

public class DeleteCustomerCommandRequest : IRequestBase<DeleteCustomerCommandResponse>
{
    public Guid Id { get; set; }
}
