using FluentValidation;

namespace VisitorApp.API.Features.Catalog.Products.Delete;

public class DeleteProductValidator : Validator<DeleteProductRequest>
{
    public DeleteProductValidator()
    {
        RuleFor(t => t.Id).NotEmpty();
    }
}