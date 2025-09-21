namespace VisitorApp.API.Features.Catalog.Categories.Create;

public class CreateCategoryRequest() : RequestBase("Categories")
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
} 