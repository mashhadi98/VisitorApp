using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Orders.ConfirmOrder;

public class ConfirmOrderRequest() : RequestBase("Orders/{Id}/confirm", ApiTypes.Post)
{
    public Guid Id { get; set; }
}

