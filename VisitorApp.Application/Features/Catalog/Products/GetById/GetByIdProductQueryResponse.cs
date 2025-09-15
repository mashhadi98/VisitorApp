namespace VisitorApp.Application.Features.Catalog.Products.GetById;

public class GetByIdProductQueryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}