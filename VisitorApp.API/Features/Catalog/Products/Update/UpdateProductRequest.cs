namespace VisitorApp.API.Features.Catalog.Products.Update;

public class UpdateProductRequest() : RequestBase("Products/{Id}")
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public Guid? CategoryId { get; set; }
}