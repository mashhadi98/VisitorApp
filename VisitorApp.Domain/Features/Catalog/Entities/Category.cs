using VisitorApp.Domain.Common.Entities;

namespace VisitorApp.Domain.Features.Catalog.Entities;

public class Category : Entity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public Category() { }

    public Category(string name, string? description = null)
    {
        Name = name;
        Description = description;
        IsActive = true;
    }

    public void UpdateDetails(string name, string? description = null)
    {
        Name = name;
        Description = description;
        SetUpdated();
    }

    public void UpdateDescription(string newDescription)
    {
        Description = newDescription;
        SetUpdated();
    }

    public void Activate()
    {
        IsActive = true;
        SetUpdated();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdated();
    }
} 