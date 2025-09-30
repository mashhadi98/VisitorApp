using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Orders.ConfirmOrder;

namespace VisitorApp.Application.Features.Orders.ConfirmOrder.Validators;

public class ConfirmOrderCommandValidator : IValidatorService<ConfirmOrderCommandRequest, ConfirmOrderCommandResponse>
{
    public void Execute(ConfirmOrderCommandRequest request)
    {

    }
}

