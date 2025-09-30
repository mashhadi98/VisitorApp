using FluentValidation;
using VisitorApp.Contract.Features.Catalog.Products.Create;

namespace VisitorApp.API.Features.Catalog.Products.Create;

public class CreateProductValidator : Validator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(t => t.Title).NotEmpty();

    }
}