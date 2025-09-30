using VisitorApp.Contract.Features.Customers.GetPaginated;
using VisitorApp.Domain.Features.Customers.Entities;

namespace VisitorApp.Application.Features.Customers.GetPaginated;

public class GetPaginatedCustomerQueryHandler(IRepository<Customer> repository) 
    : RequestHandlerBase<GetPaginatedCustomerQueryRequest, PaginatedResponse<GetPaginatedCustomerQueryResponse>>()
{
    public override Task<PaginatedResponse<GetPaginatedCustomerQueryResponse>> Handler(GetPaginatedCustomerQueryRequest request, CancellationToken cancellationToken)
    {
        Expression<Func<Customer, bool>> predicate = x => true;
        
        if (request.Filter != null)
        {
            if (!string.IsNullOrWhiteSpace(request.Filter.FullName))
            {
                predicate = predicate.And(c => c.FullName.Contains(request.Filter.FullName));
            }

            if (!string.IsNullOrWhiteSpace(request.Filter.PhoneNumber))
            {
                predicate = predicate.And(c => c.PhoneNumber.Contains(request.Filter.PhoneNumber));
            }

            if (!string.IsNullOrWhiteSpace(request.Filter.CompanyName))
            {
                predicate = predicate.And(c => c.CompanyName != null && c.CompanyName.Contains(request.Filter.CompanyName));
            }

            if (request.Filter.IsTemporary != null)
            {
                predicate = predicate.And(c => c.IsTemporary == request.Filter.IsTemporary);
            }
        }

        var result = repository.GetListPaginatedAsync(request, predicate, x => new GetPaginatedCustomerQueryResponse
        {
            Id = x.Id,
            FullName = x.FullName,
            PhoneNumber = x.PhoneNumber,
            CompanyName = x.CompanyName,
            IsTemporary = x.IsTemporary
        }, cancellationToken);

        return result;
    }
}
