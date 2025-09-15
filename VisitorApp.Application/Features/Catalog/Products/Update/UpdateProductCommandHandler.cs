using VisitorApp.Application.Common.Messaging;
using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog.Products.Update;

public class UpdateProductCommandHandler(IRepository<Product> repository, IMapper _mapper, IEnumerable<IValidatorService<UpdateProductCommandRequest, UpdateProductCommandResponse>> validators) : RequestHandlerBase<UpdateProductCommandRequest, UpdateProductCommandResponse>(validators)
{
    public override async Task<UpdateProductCommandResponse> Handler(UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {

        var item = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (item == null)
        {
            throw new ArgumentException("Product not found", nameof(request.Id));
        }
        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            item.Title = request.Title;
        }
        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            item.Description = request.Description;
        }
        if (request.IsActive != null)
        {
            item.IsActive = request.IsActive ?? false;
        }

        await repository.UpdateAsync(entity: item, autoSave: true, cancellationToken: cancellationToken);
        var result = _mapper.Map<UpdateProductCommandResponse>(item);

        return result;
    }
}