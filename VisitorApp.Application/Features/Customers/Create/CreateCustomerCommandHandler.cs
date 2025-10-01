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
        var customer = new Customer
        {
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            CompanyName = request.CompanyName,
            IsTemporary = true
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
