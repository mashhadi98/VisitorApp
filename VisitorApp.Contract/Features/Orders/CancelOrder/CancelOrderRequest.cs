using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Orders.CancelOrder;

public class CancelOrderRequest() : RequestBase("Orders/{Id}/cancel", ApiTypes.Post)
{
    public Guid Id { get; set; }
}

