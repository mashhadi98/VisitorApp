namespace VisitorApp.Contract.Features.Orders.QuickOrder;

public class QuickOrderCommandResponse
{
    public Guid OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public int Status { get; set; }
    public decimal TotalAmount { get; set; }
}

