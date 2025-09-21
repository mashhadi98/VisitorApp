namespace VisitorApp.API.Features.Catalog.Categories.Update;

public class UpdateCategoryRequest() : RequestBase("Categories/{Id}")
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
} 