using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Domain.Features.Catalog.Interfaces;

public interface IProductService
{
    Task<Guid> CreateAsync(string name, string description, CancellationToken ct = default);
    Task SoftDeleteAsync(Guid id, CancellationToken ct = default);
    Task BulkInsertAsync(IEnumerable<Product> items, CancellationToken ct = default);
}