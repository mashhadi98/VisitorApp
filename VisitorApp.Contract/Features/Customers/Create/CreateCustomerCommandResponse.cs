namespace VisitorApp.Contract.Features.Customers.Create;

public class CreateCustomerCommandResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public bool IsTemporary { get; set; }
}
