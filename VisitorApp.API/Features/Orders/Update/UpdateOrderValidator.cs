using FluentValidation;
using VisitorApp.Contract.Features.Orders.Update;

namespace VisitorApp.API.Features.Orders.Update;

public class UpdateOrderValidator : Validator<UpdateOrderRequest>
{
    public UpdateOrderValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه سفارش الزامی است");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری الزامی است");

        RuleFor(x => x.OrderDate)
            .NotEmpty().WithMessage("تاریخ سفارش الزامی است");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("حداقل یک آیتم برای سفارش الزامی است")
            .Must(items => items != null && items.Count > 0).WithMessage("سفارش باید حداقل یک آیتم داشته باشد");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("شناسه محصول الزامی است");

            item.RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("تعداد باید بزرگتر از صفر باشد");

            item.RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("قیمت واحد نمی‌تواند منفی باشد");
        });
    }
}
