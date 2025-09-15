namespace VisitorApp.API.Features.Catalog.Products.Delete;

public class DeleteProductRequest() : RequestBase("Products/{Id}")
{
    public Guid Id { get; set; }
}