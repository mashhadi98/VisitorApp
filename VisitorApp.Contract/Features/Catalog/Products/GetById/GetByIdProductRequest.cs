using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Catalog.Products.GetById;

public class GetByIdProductRequest() : RequestBase("Products/{Id}", ApiTypes.Get)
{
    public Guid Id { get; set; }
}
