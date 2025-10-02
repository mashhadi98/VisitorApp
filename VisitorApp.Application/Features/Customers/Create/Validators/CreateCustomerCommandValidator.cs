using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Customers.Create;

namespace VisitorApp.Application.Features.Customers.Create.Validators;

public class CreateCustomerCommandValidator : IValidatorService<CreateCustomerCommandRequest, CreateCustomerCommandResponse>
{
    public void Execute(CreateCustomerCommandRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            throw new ArgumentException("نام کامل مشتری الزامی است");
        }

        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            throw new ArgumentException("شماره تلفن الزامی است");
        }

        // اگر مشتری دائمی است، نام شرکت الزامی است
        if (!request.IsTemporary && string.IsNullOrWhiteSpace(request.CompanyName))
        {
            throw new ArgumentException("برای مشتریان دائمی، نام شرکت الزامی است");
        }
    }
}
