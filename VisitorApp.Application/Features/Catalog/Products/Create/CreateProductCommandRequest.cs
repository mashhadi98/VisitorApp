namespace VisitorApp.Application.Features.Catalog.Products.Create;

public class CreateProductCommandRequest : IRequestBase<CreateProductCommandResponse>
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}