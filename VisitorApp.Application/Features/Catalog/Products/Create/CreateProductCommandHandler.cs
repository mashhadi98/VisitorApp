using VisitorApp.Application.Common.Messaging;
using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog.Products.Create;

public class CreateProductCommandHandler(IRepository<Product> repository, IMapper _mapper, IEnumerable<IValidatorService<CreateProductCommandRequest, CreateProductCommandResponse>> validators) : RequestHandlerBase<CreateProductCommandRequest, CreateProductCommandResponse>(validators)
{
    public override async Task<CreateProductCommandResponse> Handler(CreateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var item = _mapper.Map<Product>(request);
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        await repository.AddAsync(entity: item, autoSave: true, cancellationToken: cancellationToken);

        var result = _mapper.Map<CreateProductCommandResponse>(item);
        return result;
    }
}