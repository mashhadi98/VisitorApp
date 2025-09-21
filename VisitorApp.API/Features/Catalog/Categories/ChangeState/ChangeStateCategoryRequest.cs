namespace VisitorApp.API.Features.Catalog.Categories.ChangeState;

public class ChangeStateCategoryRequest() : RequestBase("Categories/{Id}/state")
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
} 