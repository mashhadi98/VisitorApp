using Microsoft.EntityFrameworkCore;
using VisitorApp.Application.Common.Interfaces;
using VisitorApp.Domain.Features.Catalog.Entities;
using VisitorApp.Domain.Features.Catalog.Interfaces;

namespace VisitorApp.Persistence.Features.Catalog.Categories.Services;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<Product> _productRepository;

    public CategoryService(IRepository<Category> categoryRepository, IRepository<Product> productRepository)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
    }

    public async Task<Guid> CreateAsync(string name, string? description = null, CancellationToken ct = default)
    {
        // Check if category with same name already exists
        var existingCategory = await _categoryRepository.GetQuery()
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower(), ct);
        
        if (existingCategory != null)
        {
            throw new InvalidOperationException("دسته بندی با این نام قبلاً وجود دارد");
        }

        var category = new Category(name, description);
        await _categoryRepository.AddAsync(category, autoSave: true, cancellationToken: ct);
        return category.Id;
    }

    public async Task UpdateAsync(Guid id, string name, string? description = null, CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("دسته بندی یافت نشد");
        
        // Check if another category with same name already exists
        var existingCategory = await _categoryRepository.GetQuery()
            .FirstOrDefaultAsync(c => c.Id != id && c.Name.ToLower() == name.ToLower(), ct);
        
        if (existingCategory != null)
        {
            throw new InvalidOperationException("دسته بندی با این نام قبلاً وجود دارد");
        }

        category.UpdateDetails(name, description);
        await _categoryRepository.UpdateAsync(category, autoSave: true, cancellationToken: ct);
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("دسته بندی یافت نشد");
        
        // Check if category has related products
        var hasProducts = await _productRepository.GetQuery()
            .AnyAsync(p => p.CategoryId == id, ct);
        
        if (hasProducts)
        {
            throw new InvalidOperationException("این دسته بندی دارای محصول مرتبط می باشد و قابل حذف نیست");
        }

        await _categoryRepository.DeleteAsync(category, autoSave: true, cancellationToken: ct);
    }

    public async Task ChangeStateAsync(Guid id, bool isActive, CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("دسته بندی یافت نشد");
        
        if (isActive)
            category.Activate();
        else
            category.Deactivate();
            
        await _categoryRepository.UpdateAsync(category, autoSave: true, cancellationToken: ct);
    }

    public async Task BulkInsertAsync(IEnumerable<Category> items, CancellationToken ct = default)
    {
        await _categoryRepository.AddRangeAsync(items, autoSave: true, cancellationToken: ct);
    }
} 