using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Catalog.Categories.GetById;

public class GetByIdCategoryRequest() : RequestBase("Categories/{Id}", ApiTypes.Get)
{
    public Guid Id { get; set; }
}
