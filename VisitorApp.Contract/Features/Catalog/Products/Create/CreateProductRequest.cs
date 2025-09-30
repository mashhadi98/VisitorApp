using Microsoft.AspNetCore.Http;
using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Catalog.Products.Create;

public class CreateProductRequest() : RequestBase("Products", ApiTypes.Post)
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal Price { get; set; }
    
    // Image upload properties
    public IFormFile? ImageFile { get; set; }
}
