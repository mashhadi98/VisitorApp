using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Domain.Features.Catalog.Interfaces;

public interface ICategoryService
{
    Task<Guid> CreateAsync(string name, string? description = null, CancellationToken ct = default);
    Task UpdateAsync(Guid id, string name, string? description = null, CancellationToken ct = default);
    Task SoftDeleteAsync(Guid id, CancellationToken ct = default);
    Task ChangeStateAsync(Guid id, bool isActive, CancellationToken ct = default);
    Task BulkInsertAsync(IEnumerable<Category> items, CancellationToken ct = default);
} 