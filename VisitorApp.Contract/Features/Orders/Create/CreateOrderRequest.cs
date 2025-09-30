using VisitorApp.Contract.Common;
using VisitorApp.Contract.Features.Orders.Common;

namespace VisitorApp.Contract.Features.Orders.Create;

public class CreateOrderRequest() : RequestBase("Orders", ApiTypes.Post)
{
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public int Status { get; set; } = 0; // Pending by default
    public List<OrderItemDto> Items { get; set; } = new();
}
