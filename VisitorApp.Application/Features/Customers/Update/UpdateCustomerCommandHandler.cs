using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Customers.Update;
using VisitorApp.Domain.Features.Customers.Entities;

namespace VisitorApp.Application.Features.Customers.Update;

public class UpdateCustomerCommandHandler(
    IRepository<Customer> repository,
    IEnumerable<IValidatorService<UpdateCustomerCommandRequest, UpdateCustomerCommandResponse>> validators) 
    : RequestHandlerBase<UpdateCustomerCommandRequest, UpdateCustomerCommandResponse>(validators)
{
    public override async Task<UpdateCustomerCommandResponse> Handler(UpdateCustomerCommandRequest request, CancellationToken cancellationToken)
    {
        var customer = await repository.GetByIdAsync(request.Id, cancellationToken);
        
        if (customer == null)
        {
            throw new ArgumentException("مشتری یافت نشد", nameof(request.Id));
        }

        customer.FullName = request.FullName;
        customer.PhoneNumber = request.PhoneNumber;
        customer.CompanyName = request.CompanyName;
        customer.IsTemporary = request.IsTemporary;

        await repository.UpdateAsync(entity: customer, autoSave: true, cancellationToken: cancellationToken);

        var result = new UpdateCustomerCommandResponse
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
