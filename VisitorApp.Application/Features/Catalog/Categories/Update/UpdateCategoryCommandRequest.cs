namespace VisitorApp.Application.Features.Catalog.Categories.Update;

public class UpdateCategoryCommandRequest : IRequestBase<UpdateCategoryCommandResponse>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
} 