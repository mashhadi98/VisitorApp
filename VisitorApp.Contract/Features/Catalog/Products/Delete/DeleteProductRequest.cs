using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Catalog.Products.Delete;

public class DeleteProductRequest() : RequestBase("Products/{Id}", ApiTypes.Delete)
{
    public Guid Id { get; set; }
}
