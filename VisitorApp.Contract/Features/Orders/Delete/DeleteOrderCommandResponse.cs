namespace VisitorApp.Contract.Features.Orders.Delete;

public class DeleteOrderCommandResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
