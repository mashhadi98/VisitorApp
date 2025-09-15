namespace VisitorApp.Application.Features.Catalog.Products.Update;

public class UpdateProductCommandResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
} 