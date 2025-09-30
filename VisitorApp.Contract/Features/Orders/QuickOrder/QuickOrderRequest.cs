using VisitorApp.Contract.Common;
using VisitorApp.Contract.Features.Orders.Common;

namespace VisitorApp.Contract.Features.Orders.QuickOrder;

public class QuickOrderRequest() : RequestBase("Orders/quick", ApiTypes.Post)
{
    public Guid? UserId { get; set; }
    public Guid? CustomerId { get; set; }
    public NewCustomerDto? NewCustomer { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
    public string? Note { get; set; }
}

public class NewCustomerDto
{
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public bool IsTemporary { get; set; }
}

