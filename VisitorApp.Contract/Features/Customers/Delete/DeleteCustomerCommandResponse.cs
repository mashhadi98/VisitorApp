namespace VisitorApp.Contract.Features.Customers.Delete;

public class DeleteCustomerCommandResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
