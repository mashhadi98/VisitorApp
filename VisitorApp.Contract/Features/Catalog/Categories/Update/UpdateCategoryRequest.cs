using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Catalog.Categories.Update;

public class UpdateCategoryRequest() : RequestBase("Categories/{Id}", ApiTypes.Put)
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
