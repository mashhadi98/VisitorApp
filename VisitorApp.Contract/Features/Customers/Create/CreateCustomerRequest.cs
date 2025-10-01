using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Customers.Create;

public class CreateCustomerRequest() : RequestBase("Customers", ApiTypes.Post)
{
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
}
