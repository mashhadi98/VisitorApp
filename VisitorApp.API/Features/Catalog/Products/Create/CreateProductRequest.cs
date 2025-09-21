namespace VisitorApp.API.Features.Catalog.Products.Create;

public class CreateProductRequest() : RequestBase("Products")
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal Price { get; set; }
    
    // Image upload properties
    public IFormFile? ImageFile { get; set; }
}