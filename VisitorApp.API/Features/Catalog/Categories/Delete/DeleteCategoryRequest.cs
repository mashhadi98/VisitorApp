namespace VisitorApp.API.Features.Catalog.Categories.Delete;

public class DeleteCategoryRequest() : RequestBase("Categories/{Id}")
{
    public Guid Id { get; set; }
} 