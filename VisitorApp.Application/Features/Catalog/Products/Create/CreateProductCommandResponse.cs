namespace VisitorApp.Application.Features.Catalog.Products.Create;

public class CreateProductCommandResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
} 