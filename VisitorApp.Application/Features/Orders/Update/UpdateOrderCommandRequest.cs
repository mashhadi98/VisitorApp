using VisitorApp.Contract.Features.Orders.Common;
using VisitorApp.Contract.Features.Orders.Update;

namespace VisitorApp.Application.Features.Orders.Update;

public class UpdateOrderCommandRequest : IRequestBase<UpdateOrderCommandResponse>
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public int Status { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}
