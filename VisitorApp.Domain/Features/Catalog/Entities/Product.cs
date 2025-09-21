using VisitorApp.Domain.Common.Entities;

namespace VisitorApp.Domain.Features.Catalog.Entities;

public class Product : Entity // Guid-based by default
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }
    
    // Image properties
    public string? ImagePath { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageFileName { get; set; }
    public long? ImageFileSize { get; set; }
    
    // Price
    public decimal Price { get; set; }

    public Product() { }

    public Product(string title, string description, Guid? categoryId = null)
    {
        Title = title;
        Description = description;
        CategoryId = categoryId;
        IsActive = true;
    }

    public void UpdateDescription(string newDescription)
    {
        Description = newDescription;
        SetUpdated();
    }

    public void UpdateImage(string imagePath, string imageUrl, string? fileName = null, long? fileSize = null)
    {
        ImagePath = imagePath;
        ImageUrl = imageUrl;
        ImageFileName = fileName;
        ImageFileSize = fileSize;
        SetUpdated();
    }

    public void RemoveImage()
    {
        ImagePath = null;
        ImageUrl = null;
        ImageFileName = null;
        ImageFileSize = null;
        SetUpdated();
    }

    public void UpdatePrice(decimal price)
    {
        Price = price;
        SetUpdated();
    }

    public bool HasImage => !string.IsNullOrEmpty(ImagePath);
}
