using VisitorApp.Domain.Common.Entities;

namespace VisitorApp.Domain.Features.Catalog.Entities;


public class Product : Entity // Guid-based by default
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }
    public Product() { }

    public Product(string title, string description, Guid? categoryId = null)
    {
        Title = title;
        Description = description;
        CategoryId = categoryId;
        // Removed SetCreated() - will be handled by WriteRepository
    }

    public void UpdateDescription(string newDescription)
    {
        Description = newDescription;
        SetUpdated();
    }
}
