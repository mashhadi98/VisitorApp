using VisitorApp.Domain.Common.Entities;
using VisitorApp.Domain.Features.Orders.Entities;

namespace VisitorApp.Domain.Features.Customers.Entities;

public class Customer : Entity
{
    public string FullName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string? CompanyName { get; set; }
    public bool IsTemporary { get; set; } // مشتری متفرقه
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}