namespace VisitorApp.Application.Features.Catalog.Categories.Delete;

public class DeleteCategoryCommandRequest : IRequestBase<DeleteCategoryCommandResponse>
{
    public Guid Id { get; set; }
} 