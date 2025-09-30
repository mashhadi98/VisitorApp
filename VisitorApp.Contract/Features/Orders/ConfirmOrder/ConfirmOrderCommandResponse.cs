namespace VisitorApp.Contract.Features.Orders.ConfirmOrder;

public class ConfirmOrderCommandResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

