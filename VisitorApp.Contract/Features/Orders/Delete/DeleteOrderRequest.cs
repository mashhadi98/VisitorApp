using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Orders.Delete;

public class DeleteOrderRequest() : RequestBase("Orders/{Id}", ApiTypes.Delete)
{
    public Guid Id { get; set; }
}
