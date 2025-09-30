using VisitorApp.Contract.Common;
using VisitorApp.Contract.Features.Orders.Common;

namespace VisitorApp.Contract.Features.Orders.Update;

public class UpdateOrderRequest() : RequestBase("Orders/{Id}", ApiTypes.Put)
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public int Status { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}
