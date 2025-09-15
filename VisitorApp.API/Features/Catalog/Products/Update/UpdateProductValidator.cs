using FluentValidation;

namespace VisitorApp.API.Features.Catalog.Products.Update;

public class UpdateProductValidator : Validator<UpdateProductRequest>
{
    public UpdateProductValidator()
    {
        RuleFor(t => t.Id).NotEmpty();
    }
}