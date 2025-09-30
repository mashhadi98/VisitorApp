using VisitorApp.Contract.Features.Customers.GetById;
using VisitorApp.Domain.Features.Customers.Entities;

namespace VisitorApp.Application.Features.Customers.GetById;

public class GetByIdCustomerQueryHandler(IRepository<Customer> repository) 
    : RequestHandlerBase<GetByIdCustomerQueryRequest, GetByIdCustomerQueryResponse>()
{
    public override async Task<GetByIdCustomerQueryResponse> Handler(GetByIdCustomerQueryRequest request, CancellationToken cancellationToken)
    {
        var customer = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (customer == null)
        {
            throw new ArgumentException("مشتری یافت نشد", nameof(request.Id));
        }

        var result = new GetByIdCustomerQueryResponse
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
