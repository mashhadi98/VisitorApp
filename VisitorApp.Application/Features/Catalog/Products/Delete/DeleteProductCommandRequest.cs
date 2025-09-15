namespace VisitorApp.Application.Features.Catalog.Products.Delete;

public class DeleteProductCommandRequest : IRequestBase<DeleteProductCommandResponse>
{
    public Guid Id { get; set; }
}