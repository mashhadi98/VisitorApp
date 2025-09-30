namespace VisitorApp.Contract.Features.Orders.GetPaginated;

public class GetPaginatedOrderQueryResponse
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public Guid? UserId { get; set; }
    public string? UserFullName { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public int ItemCount { get; set; }
}
