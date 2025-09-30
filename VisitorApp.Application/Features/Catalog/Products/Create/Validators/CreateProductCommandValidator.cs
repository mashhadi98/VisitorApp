using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Catalog.Products.Create;
using VisitorApp.Domain.Common.ResponseMessages;

namespace VisitorApp.Application.Features.Catalog.Products.Create.Validators;

public class CreateProductCommandValidator : IValidatorService<CreateProductCommandRequest, CreateProductCommandResponse>
{
    public void Execute(CreateProductCommandRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ArgumentException(ErrorMessages.Products.TitleNotFound);
        }
        if (string.IsNullOrWhiteSpace(request.Description))
        {
            throw new ArgumentException(ErrorMessages.Products.DescriptionNotFound);
        }
    }
}
