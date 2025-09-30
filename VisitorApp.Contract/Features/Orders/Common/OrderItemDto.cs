namespace VisitorApp.Contract.Features.Orders.Common;

public class OrderItemDto
{
    public Guid? Id { get; set; }
    public Guid ProductId { get; set; }
    public string? ProductTitle { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Quantity * UnitPrice;
}
