using FluentValidation;

namespace VisitorApp.API.Features.Catalog.Products.Create;

public class CreateProductValidator : Validator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(t => t.Title).NotEmpty();

    }
}