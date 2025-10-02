using Microsoft.EntityFrameworkCore;
using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Customers.Create;
using VisitorApp.Domain.Features.Customers.Entities;

namespace VisitorApp.Application.Features.Customers.Create;

public class CreateCustomerCommandHandler(
    IRepository<Customer> repository,
    IEnumerable<IValidatorService<CreateCustomerCommandRequest, CreateCustomerCommandResponse>> validators) 
    : RequestHandlerBase<CreateCustomerCommandRequest, CreateCustomerCommandResponse>(validators)
{
    public override async Task<CreateCustomerCommandResponse> Handler(CreateCustomerCommandRequest request, CancellationToken cancellationToken)
    {
        // بررسی تکراری بودن شماره تلفن برای مشتریان دائمی
        if (!request.IsTemporary)
        {
            var existingCustomer = await repository.GetQuery()
                .FirstOrDefaultAsync(
                    c => c.PhoneNumber == request.PhoneNumber && !c.IsTemporary,
                    cancellationToken);

            if (existingCustomer != null)
            {
                throw new InvalidOperationException($"مشتری با شماره تلفن {request.PhoneNumber} قبلاً ثبت شده است");
            }
        }

        var customer = new Customer
        {
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            CompanyName = request.CompanyName,
            IsTemporary = request.IsTemporary
        };

        await repository.AddAsync(entity: customer, autoSave: true, cancellationToken: cancellationToken);

        var result = new CreateCustomerCommandResponse
        {
            Id = customer.Id,
            FullName = customer.FullName,
            PhoneNumber = customer.PhoneNumber,
            CompanyName = customer.CompanyName,
            IsTemporary = customer.IsTemporary
        };

        return result;
    }
}
