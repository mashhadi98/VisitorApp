using VisitorApp.Contract.Features.Customers.Create;

namespace VisitorApp.Application.Features.Customers.Create;

public class CreateCustomerCommandRequest : IRequestBase<CreateCustomerCommandResponse>
{
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
}
