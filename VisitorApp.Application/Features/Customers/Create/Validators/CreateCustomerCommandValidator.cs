using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Customers.Create;

namespace VisitorApp.Application.Features.Customers.Create.Validators;

public class CreateCustomerCommandValidator : IValidatorService<CreateCustomerCommandRequest, CreateCustomerCommandResponse>
{
    public void Execute(CreateCustomerCommandRequest request)
    {

    }
}
