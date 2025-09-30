using VisitorApp.Contract.Features.Catalog.Categories.Create;

namespace VisitorApp.Application.Features.Catalog.Categories.Create;

public class CreateCategoryCommandRequest : IRequestBase<CreateCategoryCommandResponse>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
} 