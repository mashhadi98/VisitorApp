namespace VisitorApp.Application.Features.Catalog.Products.Update;

public class UpdateProductCommandResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal Price { get; set; }
    
    // Image information
    public string? ImageUrl { get; set; }
    public string? ImageFileName { get; set; }
    public long? ImageFileSize { get; set; }
    public bool HasImage { get; set; }
} 