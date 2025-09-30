using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Orders.CancelOrder;

namespace VisitorApp.Application.Features.Orders.CancelOrder.Validators;

public class CancelOrderCommandValidator : IValidatorService<CancelOrderCommandRequest, CancelOrderCommandResponse>
{
    public void Execute(CancelOrderCommandRequest request)
    {

    }
}

