namespace VisitorApp.Contract.Features.Orders.CancelOrder;

public class CancelOrderCommandResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

