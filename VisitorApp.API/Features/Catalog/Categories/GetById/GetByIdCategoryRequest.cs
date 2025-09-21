namespace VisitorApp.API.Features.Catalog.Categories.GetById;

public class GetByIdCategoryRequest() : RequestBase("Categories/{Id}")
{
    public Guid Id { get; set; }
} 