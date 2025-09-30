using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Catalog.Products.ChangeState;

public class ChangeStateProductRequest() : RequestBase("Products/ChangeState", ApiTypes.Patch)
{
    public Guid Id { get; set; }
}
