using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Customers.Delete;

namespace VisitorApp.Application.Features.Customers.Delete.Validators;

public class DeleteCustomerCommandValidator : IValidatorService<DeleteCustomerCommandRequest, DeleteCustomerCommandResponse>
{
    public void Execute(DeleteCustomerCommandRequest request)
    {

    }
}
