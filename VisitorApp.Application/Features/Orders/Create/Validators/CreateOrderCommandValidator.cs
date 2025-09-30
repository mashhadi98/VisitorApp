using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Orders.Create;

namespace VisitorApp.Application.Features.Orders.Create.Validators;

public class CreateOrderCommandValidator : IValidatorService<CreateOrderCommandRequest, CreateOrderCommandResponse>
{
    public void Execute(CreateOrderCommandRequest request)
    {

    }
}
