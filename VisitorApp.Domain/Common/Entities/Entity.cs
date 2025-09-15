using VisitorApp.Domain.Common.Contracts;

namespace VisitorApp.Domain.Common.Entities;

public abstract class Entity : EntityWithAudit<Guid>, IHasConcurrencyVersion
{
    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    public long Version { get; set; } = 0;
}