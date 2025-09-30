using VisitorApp.Contract.Features.Orders.CancelOrder;

namespace VisitorApp.Application.Features.Orders.CancelOrder;

public class CancelOrderCommandRequest : IRequestBase<CancelOrderCommandResponse>
{
    public Guid Id { get; set; }
}

