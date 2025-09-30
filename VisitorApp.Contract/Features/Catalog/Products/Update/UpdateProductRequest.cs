using Microsoft.AspNetCore.Http;
using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Catalog.Products.Update;

public class UpdateProductRequest() : RequestBase("Products/{Id}", ApiTypes.Put)
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal? Price { get; set; }
    
    // Image upload properties
    public IFormFile? ImageFile { get; set; }
    public bool? RemoveExistingImage { get; set; } = false;
}
