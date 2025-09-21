namespace VisitorApp.Application.Features.Catalog.Products.GetPaginated;

public class GetPaginatedProductQueryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; set; }
} 