using VisitorApp.Application.Common.Messaging;

namespace VisitorApp.Application.Features.Catalog.Products.Update.Validators;

public class UpdateProductCommandValidator : IValidatorService<UpdateProductCommandRequest, UpdateProductCommandResponse>
{
    public void Execute(UpdateProductCommandRequest request)
    {
        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("id is required");
        }
    }
}
