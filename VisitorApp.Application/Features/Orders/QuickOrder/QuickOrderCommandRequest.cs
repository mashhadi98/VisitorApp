using VisitorApp.Contract.Features.Orders.Common;
using VisitorApp.Contract.Features.Orders.QuickOrder;

namespace VisitorApp.Application.Features.Orders.QuickOrder;

public class QuickOrderCommandRequest : IRequestBase<QuickOrderCommandResponse>
{
    public Guid? UserId { get; set; }
    public Guid? CustomerId { get; set; }
    public NewCustomerDto? NewCustomer { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
    public string? Note { get; set; }
}

