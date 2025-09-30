using VisitorApp.Contract.Features.Orders.Common;

namespace VisitorApp.Contract.Features.Orders.GetById;

public class GetByIdOrderQueryResponse
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhoneNumber { get; set; }
    public Guid? UserId { get; set; }
    public string? UserFullName { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public List<OrderItemDto> Items { get; set; } = new();
}
