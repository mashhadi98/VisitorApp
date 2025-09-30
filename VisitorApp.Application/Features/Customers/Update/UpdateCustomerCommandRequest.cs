using VisitorApp.Contract.Features.Customers.Update;

namespace VisitorApp.Application.Features.Customers.Update;

public class UpdateCustomerCommandRequest : IRequestBase<UpdateCustomerCommandResponse>
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public bool IsTemporary { get; set; }
}
