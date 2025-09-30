using FluentValidation;
using VisitorApp.Contract.Features.Customers.Update;

namespace VisitorApp.API.Features.Customers.Update;

public class UpdateCustomerValidator : Validator<UpdateCustomerRequest>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه مشتری الزامی است");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("نام و نام خانوادگی الزامی است")
            .MaximumLength(200).WithMessage("نام و نام خانوادگی نباید بیشتر از 200 کاراکتر باشد");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("شماره تلفن الزامی است")
            .MaximumLength(20).WithMessage("شماره تلفن نباید بیشتر از 20 کاراکتر باشد");

        RuleFor(x => x.CompanyName)
            .MaximumLength(200).WithMessage("نام شرکت نباید بیشتر از 200 کاراکتر باشد")
            .When(x => !string.IsNullOrEmpty(x.CompanyName));
    }
}
