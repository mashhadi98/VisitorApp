using VisitorApp.Contract.Features.Orders.ConfirmOrder;

namespace VisitorApp.Application.Features.Orders.ConfirmOrder;

public class ConfirmOrderCommandRequest : IRequestBase<ConfirmOrderCommandResponse>
{
    public Guid Id { get; set; }
}

