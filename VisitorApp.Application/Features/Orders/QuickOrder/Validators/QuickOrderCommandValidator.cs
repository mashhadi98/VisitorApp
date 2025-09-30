using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Orders.QuickOrder;

namespace VisitorApp.Application.Features.Orders.QuickOrder.Validators;

public class QuickOrderCommandValidator : IValidatorService<QuickOrderCommandRequest, QuickOrderCommandResponse>
{
    public void Execute(QuickOrderCommandRequest request)
    {

    }
}

