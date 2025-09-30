using VisitorApp.Contract.Features.Catalog.Categories.Delete;

namespace VisitorApp.Application.Features.Catalog.Categories.Delete;

public class DeleteCategoryCommandRequest : IRequestBase<DeleteCategoryCommandResponse>
{
    public Guid Id { get; set; }
} 