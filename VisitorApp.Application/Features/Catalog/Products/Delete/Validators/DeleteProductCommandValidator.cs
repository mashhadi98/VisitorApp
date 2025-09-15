using VisitorApp.Application.Common.Messaging;

namespace VisitorApp.Application.Features.Catalog.Products.Delete.Validators;

public class DeleteProductCommandValidator : IValidatorService<DeleteProductCommandRequest, DeleteProductCommandResponse>
{
    public void Execute(DeleteProductCommandRequest request)
    {
        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("id is required");
        }
    }
}
