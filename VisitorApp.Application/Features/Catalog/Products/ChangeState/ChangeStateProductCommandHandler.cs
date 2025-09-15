using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog.Products.ChangeState;

public class ChangeStateProductCommandHandler(IRepository<Product> repository, IMapper _mapper) : RequestHandlerBase<ChangeStateProductCommandRequest, ChangeStateProductCommandResponse>
{
    public override async Task<ChangeStateProductCommandResponse> Handler(ChangeStateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(id: request.Id, cancellationToken: cancellationToken);

        if (item == null)
        {
            throw new Exception("not found");
        }

        item.IsActive = !item.IsActive;
        await repository.UpdateAsync(entity: item, autoSave: true, cancellationToken: cancellationToken);

        var result = _mapper.Map<ChangeStateProductCommandResponse>(item);
        return result;
    }
}