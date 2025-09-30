using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Catalog.Categories.Delete;

public class DeleteCategoryRequest() : RequestBase("Categories/{Id}", ApiTypes.Delete)
{
    public Guid Id { get; set; }
}
