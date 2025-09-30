namespace VisitorApp.Contract.Features.Catalog.Categories.GetPaginated;

public class GetPaginatedCategoryQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
