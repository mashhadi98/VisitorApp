using VisitorApp.Contract.Features.Orders.Common;
using VisitorApp.Contract.Features.Orders.Create;

namespace VisitorApp.Application.Features.Orders.Create;

public class CreateOrderCommandRequest : IRequestBase<CreateOrderCommandResponse>
{
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public int Status { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}
