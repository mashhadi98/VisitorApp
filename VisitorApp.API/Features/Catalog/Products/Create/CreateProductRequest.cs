namespace VisitorApp.API.Features.Catalog.Products.Create;

public class CreateProductRequest() : RequestBase("Products")
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}