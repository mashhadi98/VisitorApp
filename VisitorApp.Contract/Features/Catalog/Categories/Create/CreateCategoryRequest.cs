using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Catalog.Categories.Create;

public class CreateCategoryRequest() : RequestBase("Categories", ApiTypes.Post)
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
