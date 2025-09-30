using FluentValidation;
using VisitorApp.Contract.Features.Orders.QuickOrder;

namespace VisitorApp.API.Features.Orders.QuickOrder;

public class QuickOrderValidator : Validator<QuickOrderRequest>
{
    public QuickOrderValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("حداقل یک آیتم برای سفارش الزامی است");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("شناسه محصول الزامی است");

            item.RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("تعداد باید بزرگتر از صفر باشد");

            item.RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("قیمت واحد نمی‌تواند منفی باشد");
        });

        RuleFor(x => x)
            .Must(x => x.CustomerId.HasValue || x.NewCustomer != null)
            .WithMessage("باید CustomerId یا NewCustomer ارسال شود");

        When(x => x.NewCustomer != null, () =>
        {
            RuleFor(x => x.NewCustomer!.FullName)
                .NotEmpty().WithMessage("نام مشتری الزامی است")
                .MaximumLength(200).WithMessage("نام نباید بیشتر از 200 کاراکتر باشد");

            RuleFor(x => x.NewCustomer!.PhoneNumber)
                .NotEmpty().WithMessage("شماره تلفن الزامی است")
                .MaximumLength(20).WithMessage("شماره تلفن نباید بیشتر از 20 کاراکتر باشد");
        });
    }
}

