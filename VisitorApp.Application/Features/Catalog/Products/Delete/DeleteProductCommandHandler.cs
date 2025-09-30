using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Catalog.Products.Delete;
using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog.Products.Delete;

public class DeleteProductCommandHandler(IRepository<Product> repository, IEnumerable<IValidatorService<DeleteProductCommandRequest, DeleteProductCommandResponse>> validators) : RequestHandlerBase<DeleteProductCommandRequest, DeleteProductCommandResponse>(validators)
{
    public override async Task<DeleteProductCommandResponse> Handler(DeleteProductCommandRequest request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            throw new ArgumentException("Product not found", nameof(request.Id));
        }

        await repository.DeleteAsync(entity: product, autoSave: true, cancellationToken: cancellationToken);

        return new DeleteProductCommandResponse { Id = request.Id };
    }
}