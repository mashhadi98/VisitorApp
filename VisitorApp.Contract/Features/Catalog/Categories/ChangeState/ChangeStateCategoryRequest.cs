using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Catalog.Categories.ChangeState;

public class ChangeStateCategoryRequest() : RequestBase("Categories/{Id}/state", ApiTypes.Patch)
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
}
