namespace VisitorApp.Application.Features.Catalog.Categories.Create;

public class CreateCategoryCommandResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
} 