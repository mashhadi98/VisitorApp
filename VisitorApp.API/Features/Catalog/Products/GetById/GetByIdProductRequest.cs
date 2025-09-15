namespace VisitorApp.API.Features.Catalog.Products.GetById;

public class GetByIdProductRequest() : RequestBase("Products/{Id}")
{
    public Guid Id { get; set; }
}