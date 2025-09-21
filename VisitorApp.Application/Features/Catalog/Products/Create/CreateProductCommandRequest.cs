using Microsoft.AspNetCore.Http;

namespace VisitorApp.Application.Features.Catalog.Products.Create;

public class CreateProductCommandRequest : IRequestBase<CreateProductCommandResponse>
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal Price { get; set; }
    
    // Image upload properties
    public IFormFile? ImageFile { get; set; }
}