using VisitorApp.Application.Features.Customers.GetPaginated;
using VisitorApp.Contract.Features.Customers.GetPaginated;

namespace VisitorApp.API.Features.Customers.GetPaginated;

public class GetPaginatedCustomerHandler : GetEndpoint<GetPaginatedCustomerRequest, GetPaginatedCustomerQueryRequest, PaginatedResponse<GetPaginatedCustomerQueryResponse>>
{
    public override string? RolesAccess => "Admin";
    
    public GetPaginatedCustomerHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}
