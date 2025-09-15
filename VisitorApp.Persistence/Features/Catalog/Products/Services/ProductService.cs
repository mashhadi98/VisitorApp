using VisitorApp.Application.Common.Interfaces;
using VisitorApp.Domain.Features.Catalog.Entities;
using VisitorApp.Domain.Features.Catalog.Interfaces;

namespace VisitorApp.Persistence.Features.Catalog.Products.Services;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _repository;

    public ProductService(IRepository<Product> repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateAsync(string name, string description, CancellationToken ct = default)
    {
        var p = new Product(name, description);
        await _repository.AddAsync(p, autoSave: true, cancellationToken: ct);
        return p.Id;
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken ct = default)
    {
        var p = await _repository.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException();
        await _repository.DeleteAsync(p, autoSave: true, cancellationToken: ct);
    }

    public async Task BulkInsertAsync(IEnumerable<Product> items, CancellationToken ct = default)
    {
        await _repository.AddRangeAsync(items, autoSave: true, cancellationToken: ct);
    }
}