using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Orders.Update;

namespace VisitorApp.Application.Features.Orders.Update.Validators;

public class UpdateOrderCommandValidator : IValidatorService<UpdateOrderCommandRequest, UpdateOrderCommandResponse>
{
    public void Execute(UpdateOrderCommandRequest request)
    {

    }
}
