using VisitorApp.Contract.Features.Catalog.Categories.ChangeState;

namespace VisitorApp.Application.Features.Catalog.Categories.ChangeState;

public class ChangeStateCategoryCommandRequest : IRequestBase<ChangeStateCategoryCommandResponse>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
} 