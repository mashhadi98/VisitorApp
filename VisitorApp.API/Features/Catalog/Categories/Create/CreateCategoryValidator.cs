using FluentValidation;

namespace VisitorApp.API.Features.Catalog.Categories.Create;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("نام دسته بندی اجباری است")
            .MaximumLength(200).WithMessage("نام دسته بندی نمی تواند بیش از 200 کاراکتر باشد");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("توضیحات دسته بندی نمی تواند بیش از 1000 کاراکتر باشد");
    }
} 